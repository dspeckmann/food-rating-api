using Minio;

namespace FoodRatingApi.Services;

public class StorageService : IStorageService
{
    private readonly IMinioClient _minioClient;

    public StorageService(IConfiguration configuration)
    {
        var storageConfig = configuration.GetRequiredSection("Storage");

        _minioClient = new MinioClient()
            .WithEndpoint(storageConfig["Endpoint"], storageConfig.GetValue<int>("Port"))
            .WithCredentials(storageConfig["AccessKey"], storageConfig["SecretKey"])
            .WithSSL(storageConfig.GetValue<bool>("Ssl"))
            .Build();
    }

    public async Task<string> GetPresignedDownloadUrl(string bucketName, string objectName, int expiresIn = 300)
    {
        var args = new PresignedGetObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithExpiry(expiresIn);

        return await _minioClient.PresignedGetObjectAsync(args);
    }

    public async Task<string> GetPresignedUploadUrl(string bucketName, string objectName, int expiresIn = 300)
    {
        var args = new PresignedPutObjectArgs()
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
}
