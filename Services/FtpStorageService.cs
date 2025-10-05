using FluentFTP;

namespace SharedHosting.Services;

public class FtpStorageService : IStorageService
{
    private readonly string _host;
    private readonly string _username;
    private readonly string _password;

    public FtpStorageService(string host, string username, string password)
    {
        _host = host;
        _username = username;
        _password = password;
    }

    private async Task<FtpClient> GetClientAsync()
    {
        var client = new FtpClient(_host, _username, _password);
        await client.ConnectAsync();
        return client;
    }

    public async Task UploadAsync(string path, Stream content)
    {
        try
        {
            using var client = await GetClientAsync();
            await client.UploadAsync(content, path, FtpRemoteExists.Overwrite, true);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error uploading file to FTP {path}: {ex.Message}");
        }
    }

    public async Task<Stream> DownloadAsync(string path)
    {
        try
        {
            using var client = await GetClientAsync();
            var memoryStream = new MemoryStream();
            var success = await client.DownloadAsync(memoryStream, path);
            if (!success)
            {
                throw new FileNotFoundException($"File not found on FTP: {path}");
            }
            memoryStream.Position = 0;
            return memoryStream;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error downloading file from FTP {path}: {ex.Message}");
        }
    }

    public async Task DeleteAsync(string path)
    {
        try
        {
            using var client = await GetClientAsync();
            await client.DeleteFileAsync(path);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting file from FTP {path}: {ex.Message}");
        }
    }

    public async Task<bool> ExistsAsync(string path)
    {
        try
        {
            using var client = await GetClientAsync();
            return await client.FileExistsAsync(path);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error checking existence on FTP {path}: {ex.Message}");
        }
    }

    public async Task<string> GetUrlAsync(string path)
    {
        // For FTP, return FTP URL (not suitable for web display, but for completeness)
        return $"ftp://{_host}/{path}";
    }
}