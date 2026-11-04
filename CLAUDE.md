# c-sharp.tutorial

C# study material for a Python developer: a .NET 10 solution plus a textbook PDF generated from it.
The reader is new to C#. Favour plain, conventional C# over clever or newest-syntax code.

## Commands

The SDK lives in `~/.dotnet`; prefix commands with `export PATH=$HOME/.dotnet:$PATH` if `dotnet` is missing.

- Build: `dotnet build --warnaserror` (analyzers must stay at zero warnings)
- Format: `dotnet csharpier format .` (80 columns; keeps book listings from wrapping)
- Coverage: `scripts/coverage.sh`
- Hooks: `pre-commit run --all-files` (add `--hook-stage pre-push` for build + test)
- Test: `dotnet test`
- Run chapters: `dotnet run --project src/Tutorial -- <n>`
- Run API: `dotnet run --project src/Tutorial.Api --urls http://localhost:5239`
- Rebuild book: `uv run --no-project --with weasyprint python docs/book/build.py`

## Rules

- The book (`docs/book/book.src.html`) pulls code with `:::file path :: anchor` directives. Renaming or
  rewording a line used as an anchor breaks the build. After any source change, rebuild the book and check
  that its stated outputs still match `dotnet run`.
- Output blocks (`:::out`) in the book are copied from real runs. Re-run and update them when code changes.
- Chapter 15 drills and appendix A solutions are verified against the code. Keep them in sync.
- The textbook PDF is committed so it can be read without building.
