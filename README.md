# Smart File Manager

A modern Windows desktop application built with **C#**, **WinUI 3**, and **Windows App SDK**.

The application focuses on two practical file-management tools:

- **Batch Rename**: preview and rename many files safely.
- **Duplicate File Finder**: detect duplicate files by size and SHA-256 hash.

## Tech Stack

- C#
- .NET 8
- WinUI 3
- Windows App SDK
- CommunityToolkit.Mvvm

## Project Goals

Smart File Manager is designed to be fast, clean, and safe. It avoids destructive operations without confirmation, keeps the interface responsive, and separates UI logic from services.

## Main Features

### Batch Rename

- Select a folder.
- Preview old and new names before applying changes.
- Add prefixes and suffixes.
- Number files automatically.
- Replace text in filenames.
- Keep file extensions safe.

### Duplicate File Finder

- Select a folder.
- Group files by size first.
- Hash only matching-size candidates.
- Use SHA-256 for content comparison.
- Show duplicate groups and wasted storage.

## Repository Structure

```text
SmartFileManager/
├── SmartFileManager/
│   ├── Models/
│   ├── Services/
│   ├── Helpers/
│   ├── Assets/
│   ├── App.xaml
│   ├── MainWindow.xaml
│   └── SmartFileManager.csproj
└── SmartFileManager.Tests/
```

## Run

```powershell
cd SmartFileManager
dotnet restore
dotnet build
dotnet run
```

If WinUI asks for Developer Mode, enable it from Windows Settings → For developers → Developer Mode.

## Author

Built by **Radwan Abdulhadi Ahmed**.