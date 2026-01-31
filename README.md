# Sort.IO

Sort.IO is a small console tool that keeps your **Downloads** folder clean and organized.
It automatically moves files into the appropriate system folders based on their type
(Pictures, Documents, Music, Videos, etc.).

---

## Features

- 🗂️ Automatic sorting from `Downloads` into system libraries
- ✅ Duplicate-safe: appends `_1`, `_2`, ... if the file already exists
- 📁 Path check screen (shows status of system folders)
- 🧩 Simple, clean console UI
- 🔧 Built with .NET 8 and C#

---

## Project structure

- `DownloadSort.sln` — solution at the repository root (open this in your IDE)
- `DownloadSort/` — project sources and `DownloadSort.csproj`

> Note: removed nested `DownloadSort/DownloadSort.sln` to avoid confusion.

## Developer setup

- Make sure that the .NET SDK is installed and available on your PATH. On macOS, this is typically at `/usr/local/share/dotnet`.
- If VS Code shows the message "/bin/sh: dotnet: command not found", restart VS Code after you updated your shell profile (e.g. add `export PATH="$PATH:/usr/local/share/dotnet"` to `~/.zprofile` or launch VS Code from a terminal using `code .` so it inherits your shell PATH).
- This repository workspace also hides `bin/`, `obj/` and `publish/` in the Explorer via `.vscode/settings.json` to avoid distracting highlighting.

## Usage

### Run from source

```bash
dotnet run --project DownloadSort
```

### Run published build (Windows)

After publishing with:

```bash
dotnet publish -c Release -r win-x64 -o publish
```

run:

```bash
cd publish
./DownloadSort.exe
```

## License

MIT
