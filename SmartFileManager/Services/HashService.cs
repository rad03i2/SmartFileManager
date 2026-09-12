using System.Security.Cryptography;

namespace SmartFileManager.Services;

public sealed class HashService
{
    public async Task<string> ComputeSha256Async(string filePath)
    {
        await using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 1024 * 128, useAsync: true);
        var hash = await SHA256.HashDataAsync(stream);
        return Convert.ToHexString(hash);
    }
}
