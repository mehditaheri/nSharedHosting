namespace SharedHosting.Services;

public class LocalStorageService : IStorageService
{
    private readonly string _basePath;

    public LocalStorageService(string basePath)
    {
        _basePath = basePath;
        Directory.CreateDirectory(_basePath);
    }

    public async Task UploadAsync(string path, Stream content)
    {
        try
        {
            var fullPath = Path.Combine(_basePath, path);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            using var fileStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            await content.CopyToAsync(fileStream);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error uploading file to {path}: {ex.Message}");
        }
    }

    public async Task<Stream> DownloadAsync(string path)
    {
        try
        {
            var fullPath = Path.Combine(_basePath, path);
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"File not found: {path}");
            }
            return new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error downloading file from {path}: {ex.Message}");
        }
    }

    public async Task DeleteAsync(string path)
    {
        try
        {
            var fullPath = Path.Combine(_basePath, path);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting file {path}: {ex.Message}");
        }
    }

    public async Task<bool> ExistsAsync(string path)
    {
        try
        {
            var fullPath = Path.Combine(_basePath, path);
            return File.Exists(fullPath);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error checking existence of {path}: {ex.Message}");
        }
    }

    public async Task<string> GetUrlAsync(string path)
    {
        // Assuming files are served from /files/ relative to wwwroot
        return $"/files/{path.Replace('\\', '/')}";
    }
}