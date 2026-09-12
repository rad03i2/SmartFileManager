namespace SmartFileManager.Models;

public sealed record DuplicateGroup(string Hash, long Size, IReadOnlyList<DuplicateFile> Files)
{
    public long WastedBytes => Math.Max(0, Files.Count - 1) * Size;
}
