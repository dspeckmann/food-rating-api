using FoodRatingApi.Dtos;
using FoodRatingApi.Entities;

namespace FoodRatingApi.Services;

public class FoodRatingDtoMapper : IFoodRatingDtoMapper
{
    private readonly IStorageService _storageService;

    public FoodRatingDtoMapper(IStorageService storageService)
    {
        _storageService = storageService;
    }

    public async Task<FoodDto> MakeFoodDto(Food food, bool? isRatedWell = null, DateTime? lastRatingDate = null)
    {
        var pictureDto = food.Picture is not null ? await MakePictureDto(food.Picture) : null;
        return new FoodDto(
            food.Id,
            food.Name,
            pictureDto,
            food.CreatedAt,
            food.UpdatedAt,
            isRatedWell,
            lastRatingDate);
    }

    public async Task<PetDto> MakePetDto(Pet pet)
    {
        var picture = pet.Picture is not null ? await MakePictureDto(pet.Picture) : null;
        return new PetDto(pet.Id, pet.Name, picture, pet.CreatedAt, pet.UpdatedAt);
    }

    public async Task<PictureDto> MakePictureDto(Picture picture)
    {
        var downloadUrl = await _storageService.GetPresignedDownloadUrl(StorageBucketNames.Pictures, picture.ObjectName);
        return new PictureDto(picture.Id, downloadUrl);
    }

    public async Task<RatingDto> MakeRatingDto(FoodRating rating)
    {
        var pet = await MakePetDto(rating.Pet!);
        var food = await MakeFoodDto(rating.Food!);
        var picture = rating.Picture is not null ? await MakePictureDto(rating.Picture) : null;
        return new RatingDto(pet, food, rating.Taste, rating.Wellbeing, rating.Comment, picture, rating.CreatedAt);
    }
}
