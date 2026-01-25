namespace Media.Core.Services;

public interface IFileStorageService
{
    Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken cancellationToken = default);
    Task<Stream?> DownloadAsync(string storagePath, CancellationToken cancellationToken = default);
    Task DeleteAsync(string storagePath, CancellationToken cancellationToken = default);
    string GetPublicUrl(string storagePath);
}
