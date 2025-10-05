using Amazon.S3;
using Amazon.S3.Model;

namespace SharedHosting.Services;

public class S3StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public S3StorageService(IAmazonS3 s3Client, string bucketName)
    {
        _s3Client = s3Client;
        _bucketName = bucketName;
    }

    public async Task UploadAsync(string path, Stream content)
    {
        try
        {
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = path,
                InputStream = content
            };
            await _s3Client.PutObjectAsync(request);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error uploading file to S3 {path}: {ex.Message}");
        }
    }

    public async Task<Stream> DownloadAsync(string path)
    {
        try
        {
            var request = new GetObjectRequest
            {
                BucketName = _bucketName,
                Key = path
            };
            var response = await _s3Client.GetObjectAsync(request);
            return response.ResponseStream;
        }
        catch (AmazonS3Exception ex) when (ex.ErrorCode == "NoSuchKey")
        {
            throw new FileNotFoundException($"File not found on S3: {path}");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error downloading file from S3 {path}: {ex.Message}");
        }
    }

    public async Task DeleteAsync(string path)
    {
        try
        {
            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = path
            };
            await _s3Client.DeleteObjectAsync(request);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error deleting file from S3 {path}: {ex.Message}");
        }
    }

    public async Task<bool> ExistsAsync(string path)
    {
        try
        {
            var request = new GetObjectMetadataRequest
            {
                BucketName = _bucketName,
                Key = path
            };
            var response = await _s3Client.GetObjectMetadataAsync(request);
            return true;
        }
        catch (AmazonS3Exception ex) when (ex.ErrorCode == "NotFound")
        {
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error checking existence on S3 {path}: {ex.Message}");
        }
    }

    public async Task<string> GetUrlAsync(string path)
    {
        // For S3, generate pre-signed URL
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = path,
            Expires = DateTime.UtcNow.AddHours(1) // 1 hour expiry
        };
        return _s3Client.GetPreSignedURL(request);
    }
}