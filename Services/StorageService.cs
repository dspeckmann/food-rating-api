using Microsoft.Extensions.Caching.Memory;
using Minio;

namespace FoodRatingApi.Services;

public class StorageService : IStorageService
{
    // Presigned download URLs will be valid for 7 days.
    private const int PresignedDownloadUrlValidity = 604800;
    // This service will cache them for 6 days only to avoid serving outdated ones.
    private const int MemoryCacheValidity = 518400;
    
    private readonly IMinioClient _minioClient;
    private readonly IMemoryCache _cache;

    public StorageService(IConfiguration configuration, IMemoryCache cache)
    {
        var storageConfig = configuration.GetRequiredSection("Storage");

        _minioClient = new MinioClient()
            .WithEndpoint(storageConfig["Endpoint"], storageConfig.GetValue<int>("Port"))
            .WithCredentials(storageConfig["AccessKey"], storageConfig["SecretKey"])
            .WithSSL(storageConfig.GetValue<bool>("Ssl"))
            .Build();

        _cache = cache;
    }

    public async Task<string> GetPresignedDownloadUrl(string bucketName, string objectName, int expiresIn = 300)
    {
        return await _cache.GetOrCreateAsync(new { bucketName, objectName }, async entry =>
        {
            entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(MemoryCacheValidity));

            var args = new PresignedGetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithExpiry(expiresIn);

            return await _minioClient.PresignedGetObjectAsync(args);
        }) ?? string.Empty; // TODO: Why is this necessary? Can PresignedGetObjectAsync return null?
    }

    public async Task<string> GetPresignedUploadUrl(string bucketName, string objectName, int expiresIn = 300)
    {
        await CreateBucketIfItDoesNotExist(bucketName);
        var args = new PresignedPutObjectArgs()
            {
                IsBucketCreationRequest = true
            }
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithExpiry(expiresIn);

        return await _minioClient.PresignedPutObjectAsync(args);
    }

    public async Task DeleteObject(string bucketName, string objectName)
    {
        var args = new RemoveObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName);

        await _minioClient.RemoveObjectAsync(args);
    }

    private async Task CreateBucketIfItDoesNotExist(string bucketName)
    {
        // TODO: Is this cached automatically?
        var bucketExistsArgs = new BucketExistsArgs()
            .WithBucket(bucketName);

        if (!await _minioClient.BucketExistsAsync(bucketExistsArgs))
        {
            var makeBucketArgs = new MakeBucketArgs()
                .WithBucket(bucketName);
            await _minioClient.MakeBucketAsync(makeBucketArgs);
        }
    }
}
