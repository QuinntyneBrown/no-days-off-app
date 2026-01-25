using Media.Core.Services;

namespace Media.Infrastructure.Services;

public class LocalFileStorageService : IFileStorageService
{
    private readonly string _basePath;
    private readonly string _baseUrl;

    public LocalFileStorageService(string basePath, string baseUrl)
    {
        _basePath = basePath;
        _baseUrl = baseUrl;
        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        var datePath = DateTime.UtcNow.ToString("yyyy/MM/dd");
        var directoryPath = Path.Combine(_basePath, datePath);
        Directory.CreateDirectory(directoryPath);

        var filePath = Path.Combine(directoryPath, fileName);
        using var fileStream = File.Create(filePath);
        await stream.CopyToAsync(fileStream, cancellationToken);

        return Path.Combine(datePath, fileName).Replace("\\", "/");
    }

    public async Task<Stream?> DownloadAsync(string storagePath, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_basePath, storagePath);
        if (!File.Exists(fullPath))
            return null;

        var memoryStream = new MemoryStream();
        using var fileStream = File.OpenRead(fullPath);
        await fileStream.CopyToAsync(memoryStream, cancellationToken);
        memoryStream.Position = 0;
        return memoryStream;
    }

    public Task DeleteAsync(string storagePath, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_basePath, storagePath);
        if (File.Exists(fullPath))
            File.Delete(fullPath);
        return Task.CompletedTask;
    }

    public string GetPublicUrl(string storagePath)
    {
        return $"{_baseUrl.TrimEnd('/')}/{storagePath}";
    }
}
