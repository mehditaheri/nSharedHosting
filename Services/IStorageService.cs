namespace SharedHosting.Services;

public interface IStorageService
{
    Task UploadAsync(string path, Stream content);
    Task<Stream> DownloadAsync(string path);
    Task DeleteAsync(string path);
    Task<bool> ExistsAsync(string path);
    Task<string> GetUrlAsync(string path);
}