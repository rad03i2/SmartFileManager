namespace SmartFileManager.Models;

public sealed record DuplicateFile(
    string FullPath,
    string Name,
    long Size,
    DateTime LastModifiedUtc,
    string Hash);
