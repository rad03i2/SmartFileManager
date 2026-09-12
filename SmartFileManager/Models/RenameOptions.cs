namespace SmartFileManager.Models;

public sealed record RenameOptions(
    string Prefix,
    int StartNumber,
    int Padding,
    string? Suffix = null,
    string? ReplaceFrom = null,
    string? ReplaceTo = null);
