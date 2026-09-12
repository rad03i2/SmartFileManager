using SmartFileManager.Models;
using System.IO;
using System.Text.RegularExpressions;

namespace SmartFileManager.Services;

public sealed class RenameService
{
    private static readonly char[] InvalidChars = Path.GetInvalidFileNameChars();

    public IReadOnlyList<FileRenamePreview> CreatePreview(string folderPath, RenameOptions options)
    {
        if (!Directory.Exists(folderPath))
            throw new DirectoryNotFoundException(folderPath);

        var files = Directory.EnumerateFiles(folderPath).OrderBy(x => x).ToList();
        var result = new List<FileRenamePreview>();
        var plannedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        for (int index = 0; index < files.Count; index++)
        {
            var path = files[index];
            var oldName = Path.GetFileName(path);
            var extension = Path.GetExtension(path);
            var originalName = Path.GetFileNameWithoutExtension(path);
            var name = originalName;

            if (!string.IsNullOrWhiteSpace(options.ReplaceFrom))
                name = name.Replace(options.ReplaceFrom, options.ReplaceTo ?? string.Empty, StringComparison.OrdinalIgnoreCase);

            var number = (options.StartNumber + index).ToString().PadLeft(options.Padding, '0');
            var newName = $"{options.Prefix}{number}{options.Suffix}{extension}";
            newName = CleanFileName(newName);

            var status = plannedNames.Add(newName) ? "Ready" : "Duplicate target name";
            result.Add(new FileRenamePreview(path, oldName, newName, status));
        }

        return result;
    }

    public async Task<IReadOnlyList<FileRenamePreview>> ApplyAsync(string folderPath, RenameOptions options)
    {
        var preview = CreatePreview(folderPath, options);
        var output = new List<FileRenamePreview>();

        foreach (var item in preview)
        {
            if (item.Status != "Ready")
            {
                output.Add(item);
                continue;
            }

            try
            {
                var targetPath = Path.Combine(Path.GetDirectoryName(item.FullPath)!, item.NewName);
                if (File.Exists(targetPath))
                {
                    output.Add(item with { Status = "Target exists" });
                    continue;
                }

                await Task.Run(() => File.Move(item.FullPath, targetPath));
                output.Add(item with { Status = "Done" });
            }
            catch (Exception ex)
            {
                output.Add(item with { Status = ex.Message });
            }
        }

        return output;
    }

    private static string CleanFileName(string value)
    {
        foreach (var invalid in InvalidChars)
            value = value.Replace(invalid, '_');

        return Regex.Replace(value, "_+", "_");
    }
}
