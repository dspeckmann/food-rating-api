namespace FoodRatingApi.Services
{
    public interface IStorageService
    {
        Task<string> GetPresignedDownloadUrl(string bucketName, string objectName, int expiresIn = 300);
        Task<string> GetPresignedUploadUrl(string bucketName, string objectName, int expiresIn = 300);
        Task DeleteObject(string bucketName, string objectName);
    }
}