"""Build the C# textbook PDF from book.src.html.

Preprocessor directives (each on its own line in book.src.html):

  :::cs [label]        ... :::    C# block written inline
  :::py [label]        ... :::    Python block written inline
  :::out [label]       ... :::    program output
  :::file PATH :: ANCHOR [:: ANCHOR ...]
      Excerpt from a real source file. For each ANCHOR, take the first line
      containing it, the comment lines directly above it, and the rest of
      its { } block (or just the line, if it ends with ';').

Run from the repo root:
  uv run --no-project --with weasyprint python docs/book/build.py
"""

import html
import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]
HERE = Path(__file__).resolve().parent

CS_KW = set("""abstract as async await base bool break case catch char class const continue
decimal default do double else enum false finally for foreach get if in init int interface
internal is long namespace new null object out override params private protected public
readonly record ref return sealed set static string struct switch this throw true try typeof
using var virtual void when where while from select orderby descending group by""".split())
PY_KW = set("""and as async await class def elif else except False finally for from if import
in is lambda None not or pass raise return self True try while with yield match case""".split())

TOKEN = {
    "cs": re.compile(r'(?P<cm>//[^\n]*)|(?P<st>\$?@?"(?:[^"\\\n]|\\.)*"|\'(?:[^\'\\\n]|\\.)\')'
                     r'|(?P<id>[A-Za-z_][A-Za-z0-9_]*)'),
    "py": re.compile(r'(?P<cm>#[^\n]*)|(?P<st>[fr]?"(?:[^"\\\n]|\\.)*"|[fr]?\'(?:[^\'\\\n]|\\.)*\')'
                     r'|(?P<id>[A-Za-z_][A-Za-z0-9_]*)'),
}


def highlight(code: str, lang: str) -> str:
    if lang not in TOKEN:
        return html.escape(code)
    kws = CS_KW if lang == "cs" else PY_KW
    out, pos = [], 0
    for m in TOKEN[lang].finditer(code):
        out.append(html.escape(code[pos:m.start()]))
        text = html.escape(m.group())
        if m.lastgroup == "cm":
            out.append(f'<span class="cm">{text}</span>')
        elif m.lastgroup == "st":
            out.append(f'<span class="st">{text}</span>')
        elif m.group() in kws:
            out.append(f'<span class="kw">{text}</span>')
        else:
            out.append(text)
        pos = m.end()
    out.append(html.escape(code[pos:]))
    return "".join(out)


def block(code: str, lang: str, label: str) -> str:
    code = code.strip("\n")
    hd = f'<div class="codehd {lang}">{html.escape(label)}</div>' if label else ""
    return f'<div class="codewrap">{hd}<div class="codeblock">{highlight(code, lang)}</div></div>'


def code_part(line: str) -> str:
    """The line without a trailing // comment (quotes outside comments are balanced)."""
    for m in re.finditer(r"//", line):
        if line[:m.start()].count('"') % 2 == 0:
            return line[:m.start()]
    return line


def excerpt(path: str, spec: str, after: int) -> tuple[int, int, list[str]]:
    """Return (first, last, lines) for one anchor spec, searching from line `after`.

    "A"       -> the line containing A, plus the rest of its { } block or statement.
    "A ..> B" -> from the line containing A through the line containing B, then
                 on until any braces opened in that range are closed.
    Comment and [Attribute] lines directly above the first line are included.
    """
    lines = (ROOT / path).read_text().splitlines()
    a, _, b = (x.strip() for x in spec.partition("..>"))

    def find(text: str, frm: int) -> int:
        for i in range(frm, len(lines)):
            if text in lines[i]:
                return i
        sys.exit(f"anchor not found: {path} :: {text}")

    start = find(a, after)
    first = start
    while first > 0 and lines[first - 1].strip().startswith(("//", "[")):
        first -= 1
    stop_at = find(b, start) if b else start
    depth, opened, i = 0, False, start
    while True:
        code = code_part(lines[i])
        depth += code.count("{") - code.count("}")
        opened = opened or depth > 0
        done = i >= stop_at and (
            (opened and depth <= 0) or (not opened and code.rstrip().endswith(";"))
            or (b and depth <= 0))
        if done:
            nxt = next((ln.strip() for ln in lines[i + 1:] if ln.strip()), "")
            if not (opened and nxt.startswith(("else", "catch", "finally"))):
                break
        i += 1
    return first, i, lines[first:i + 1]


def enclosing_method(path: str, first: int, indent: int) -> str | None:
    """Name of the method whose body holds line `first` (body indented by `indent`)."""
    lines = (ROOT / path).read_text().splitlines()
    for i in range(first - 1, -1, -1):
        ln = lines[i]
        if ln.strip() and len(ln) - len(ln.lstrip()) == indent - 4 and ln.strip() != "{":
            m = re.search(r"(\w+)\s*\(", ln)
            if m and re.search(r"\b(public|private|protected|internal|static)\b", ln):
                return m.group(1)
            return None
    return None


def dedent(chunk: list[str]) -> str:
    indent = min(len(ln) - len(ln.lstrip()) for ln in chunk if ln.strip())
    return "\n".join(ln[indent:] for ln in chunk)


def keep_with_next(out: list[str]) -> None:
    """A paragraph ending in ':' introduces the code below it; keep them on one page."""
    j = len(out) - 1
    while j >= 0 and not out[j].strip():
        j -= 1
    if j < 0 or not out[j].rstrip().endswith(":</p>"):
        return
    while j >= 0 and "<p>" not in out[j]:
        j -= 1
    if j >= 0:
        out[j] = out[j].replace("<p>", '<p style="break-after: avoid">', 1)


def expand(src: str) -> str:
    out, lines, i = [], src.splitlines(), 0
    while i < len(lines):
        ln = lines[i].strip()
        if ln.startswith(":::file "):
            path, *anchors = [p.strip() for p in ln[8:].split("::")]
            parts, pos, prev_end, prev_ctx = [], 0, None, None
            for spec in anchors:
                first, last, chunk = excerpt(path, spec, pos)
                indent = min(len(c) - len(c.lstrip()) for c in chunk if c.strip())
                ctx = enclosing_method(path, first, indent) if indent >= 8 else None
                if parts and first != prev_end + 1:
                    parts.append("")
                if ctx and ctx != prev_ctx:
                    parts.append(f"// in {ctx}():")
                parts.extend(dedent(chunk).split("\n"))
                pos, prev_end, prev_ctx = last + 1, last, ctx
            keep_with_next(out)
            out.append(block(dedent(parts), "cs", path.replace("src/", "")))
            i += 1
        elif re.match(r":::(cs|py|out)\b", ln):
            lang, _, label = ln[3:].partition(" ")
            default = {"cs": "C#", "py": "Python", "out": "Output"}[lang]
            body = []
            i += 1
            while lines[i].strip() != ":::":
                body.append(lines[i])
                i += 1
            keep_with_next(out)
            out.append(block("\n".join(body), lang, label or default))
            i += 1
        else:
            out.append(lines[i])
            i += 1
    return "\n".join(out)


def main() -> None:
    from weasyprint import HTML

    html_text = expand((HERE / "book.src.html").read_text())
    (HERE / "book.html").write_text(html_text)
    target = ROOT / "docs" / "CSharp-for-Python-Developers.pdf"
    HTML(str(HERE / "book.html")).write_pdf(str(target))
    print(f"wrote {target}")


if __name__ == "__main__":
    main()
