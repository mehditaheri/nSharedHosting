using Microsoft.Extensions.Configuration;

namespace SharedHosting.Services;

public class ConfigurationService : IConfigurationService
{
    private readonly IConfiguration _configuration;
    private string _storageType;

    public ConfigurationService(IConfiguration configuration)
    {
        _configuration = configuration;
        _storageType = _configuration["Storage:Type"] ?? "Local";
    }

    public Task<string> GetStorageTypeAsync()
    {
        return Task.FromResult(_storageType);
    }

    public Task SetStorageTypeAsync(string type)
    {
        _storageType = type;
        // In a real app, persist to database or config file
        return Task.CompletedTask;
    }

    public Task<StorageSettings> GetStorageSettingsAsync()
    {
        var settings = new StorageSettings
        {
            Type = _storageType,
            Local = new LocalSettings
            {
                BasePath = _configuration["Storage:Local:BasePath"] ?? "wwwroot/uploads"
            },
            Ftp = new FtpSettings
            {
                Host = _configuration["Storage:Ftp:Host"] ?? "ftp.example.com",
                Username = _configuration["Storage:Ftp:Username"] ?? "user",
                Password = _configuration["Storage:Ftp:Password"] ?? "pass"
            },
            S3 = new S3Settings
            {
                BucketName = _configuration["Storage:S3:BucketName"] ?? "mybucket",
                Region = _configuration["Storage:S3:Region"] ?? "us-east-1"
            }
        };
        return Task.FromResult(settings);
    }

    public Task UpdateStorageSettingsAsync(StorageSettings settings)
    {
        _storageType = settings.Type;
        // In a real app, update config and persist
        return Task.CompletedTask;
    }
}