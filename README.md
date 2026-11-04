# C# for Python Developers

A crash course in C# and .NET for someone who already knows Python. It has two parts:

- **The textbook**, [`docs/CSharp-for-Python-Developers.pdf`](docs/CSharp-for-Python-Developers.pdf).
  Each chapter walks through a file in this repo and shows the matching Python.
  It ends with hands-on drills and an interview quick reference.
- **The companion code**, a .NET 10 solution with three projects:
  a console app with one file per chapter, a small web API, and unit tests.

Every code listing in the book comes from these source files, and every output shown in the book was
produced by running this code.

## Quick start

```bash
# 1. Install the .NET 10 SDK (skip this if `dotnet --version` prints 10.x)
curl -sSL https://dot.net/v1/dotnet-install.sh | bash -s -- --channel 10.0
export PATH="$HOME/.dotnet:$PATH"          # add this line to ~/.bashrc too

# 2. Build, test, run
dotnet build                               # compile everything
dotnet test                                # 8 unit tests
dotnet run --project src/Tutorial          # run every chapter
dotnet run --project src/Tutorial -- 6     # run the demo for book chapter 6
```

## Layout

```
CSharpTutorial.slnx            solution: lists the three projects
global.json                    pins the .NET SDK version
src/
  Tutorial/                    console app (one file per book chapter)
    Program.cs                 entry point: picks which chapters to run
    Ch03Basics.cs ... Ch12Async.cs   one file per book chapter, same number
    Ch13Orders.cs              runs the order system end to end
    Orders/                    the order system: products, discounts, checkout
  Tutorial.Api/                ASP.NET Core minimal web API over the order system
tests/
  Tutorial.Tests/              xUnit tests for BankAccount and OrderService
docs/
  CSharp-for-Python-Developers.pdf   the textbook
  book/                        textbook source and build script
```

## Chapter map

Demo numbers match the book's chapter numbers.

| `dotnet run -- N` | File | Topic |
|---|---|---|
| 3 | `Ch03Basics.cs` | Syntax, string interpolation |
| 4 | `Ch04Types.cs` | Types, `var`, value vs reference, `null` |
| 5 | `Ch05ControlFlow.cs` | `if`, `switch` expressions, loops |
| 6 | `Ch06Collections.cs` | Arrays, `List`, `Dictionary`, `HashSet`, tuples |
| 7 | `Ch07Methods.cs` | Signatures, `out`/`ref`/`params`, lambdas |
| 8 | `Ch08Classes.cs` | Classes, properties, records, structs |
| 9 | `Ch09Interfaces.cs` | Interfaces, abstract classes, polymorphism |
| 10 | `Ch10Linq.cs` | LINQ, deferred execution, extension methods |
| 11 | `Ch11Exceptions.cs` | `try`/`catch`, custom exceptions, `using` |
| 12 | `Ch12Async.cs` | `async`/`await`, `Task.WhenAll` |
| 13 | `Ch13Orders.cs` + `Orders/` | Everything together; dependency injection |

Book chapters 14 to 17 cover the web API, debugging in VS Code, the drills, and the interview quick reference.

## The web API

```bash
dotnet run --project src/Tutorial.Api --urls http://localhost:5239

# in a second terminal
curl localhost:5239/products
curl localhost:5239/stock/MN-03
curl -X POST localhost:5239/orders -H 'Content-Type: application/json' \
     -d '{"customer":"Ada","lines":[{"sku":"MN-03","quantity":1}]}'
```

## Debugging in VS Code

Open the repo root folder in VS Code and install the recommended **C# Dev Kit** extension when prompted.
The `.dotnet` folder must be on the PATH that VS Code sees (see Quick start).

- Run and Debug view (Ctrl+Shift+D) offers three configurations: **Tutorial: one chapter** (asks for a
  chapter number), **Tutorial: all chapters**, and **Web API**. F5 builds and starts the selected one.
- F9 sets a breakpoint, F10 steps over, F11 steps into, Shift+F11 steps out.
- Tests appear in the Testing view (flask icon), each with a Debug button.

Chapter 15 of the book walks through a full session.

## Practising

Chapter 16 of the book lists nine drills, from reading a failing test to using the debugger.
Appendix A has the solutions. Work on a branch so you can compare or reset:

```bash
git switch -c drills        # do the drills here
git checkout -- .           # throw away changes to tracked files
git switch main             # back to the clean version
```

## Rebuilding the textbook

The PDF is generated from `docs/book/book.src.html`. `build.py` copies code excerpts out of the
source files, so the book always shows the current code. Rebuilding needs
[uv](https://docs.astral.sh/uv/) and the WeasyPrint system libraries (pango, cairo).

```bash
uv run --no-project --with weasyprint python docs/book/build.py
```

## Python to C# cheat sheet

| Python | C# |
|---|---|
| `f"{x:.2f}"` | `$"{x:F2}"` |
| `list`, `dict`, `set` | `List<T>`, `Dictionary<K,V>`, `HashSet<T>` |
| `[f(x) for x in xs if p(x)]` | `xs.Where(p).Select(f).ToList()` |
| `@dataclass(frozen=True)` | `record` |
| `with open(...) as f:` | `using var f = ...;` |
| `async def` / `asyncio.gather` | `async Task` / `Task.WhenAll` |
| `raise` / `except E as ex:` | `throw` / `catch (E ex)` |
| `pytest` / `pytest.raises` | `dotnet test` / `Assert.Throws<E>` |
| `pip` / PyPI | `dotnet add package` / NuGet |
