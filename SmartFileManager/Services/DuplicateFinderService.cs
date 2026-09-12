using SmartFileManager.Models;
using System.IO;

namespace SmartFileManager.Services;

public sealed class DuplicateFinderService
{
    private readonly HashService _hashService = new();

    public async Task<IReadOnlyList<DuplicateGroup>> FindDuplicatesAsync(string folderPath, CancellationToken cancellationToken = default)
    {
        if (!Directory.Exists(folderPath))
            throw new DirectoryNotFoundException(folderPath);

        var files = Directory.EnumerateFiles(folderPath, "*", SearchOption.AllDirectories)
            .Select(path => new FileInfo(path))
            .Where(file => file.Exists && file.Length > 0)
            .GroupBy(file => file.Length)
            .Where(group => group.Count() > 1)
            .ToList();

        var hashedFiles = new List<DuplicateFile>();

        foreach (var sizeGroup in files)
        {
            foreach (var file in sizeGroup)
            {
                cancellationToken.ThrowIfCancellationRequested();

                try
                {
                    var hash = await _hashService.ComputeSha256Async(file.FullName);
                    hashedFiles.Add(new DuplicateFile(file.FullName, file.Name, file.Length, file.LastWriteTimeUtc, hash));
                }
                catch
                {
                    // Skip locked or unreadable files safely.
                }
            }
        }

        return hashedFiles
            .GroupBy(file => new { file.Size, file.Hash })
            .Where(group => group.Count() > 1)
            .Select(group => new DuplicateGroup(group.Key.Hash, group.Key.Size, group.ToList()))
            .ToList();
    }
}
