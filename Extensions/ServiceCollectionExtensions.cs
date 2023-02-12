using FoodRatingApi.Services;

namespace FoodRatingApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddObjectStorage(this IServiceCollection services)
    {
        services.AddSingleton<IStorageService, StorageService>();
    }
}
