using SharedHosting.Models;

namespace SharedHosting.Services;

public interface IConfigurationService
{
    Task<string> GetStorageTypeAsync();
    Task SetStorageTypeAsync(string type);
    Task<StorageSettings> GetStorageSettingsAsync();
    Task UpdateStorageSettingsAsync(StorageSettings settings);
}

public class StorageSettings
{
    public string Type { get; set; } = "Local";
    public LocalSettings Local { get; set; } = new();
    public FtpSettings Ftp { get; set; } = new();
    public S3Settings S3 { get; set; } = new();
}

public class LocalSettings
{
    public string BasePath { get; set; } = "wwwroot/uploads";
}

public class FtpSettings
{
    public string Host { get; set; } = "ftp.example.com";
    public string Username { get; set; } = "user";
    public string Password { get; set; } = "pass";
}

public class S3Settings
{
    public string BucketName { get; set; } = "mybucket";
    public string Region { get; set; } = "us-east-1";
}