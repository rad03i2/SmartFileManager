namespace SmartFileManager.Models;

public sealed record FileRenamePreview(
    string FullPath,
    string OldName,
    string NewName,
    string Status);
