using FoodRatingApi.Dtos;
using FoodRatingApi.Entities;

namespace FoodRatingApi.Services
{
    public interface IFoodRatingDtoMapper
    {
        Task<FoodDto> MakeFoodDto(Food food, bool? isRatedWell = null, DateTime? lastRatingDate = null);
        Task<PetDto> MakePetDto(Pet pet);
        Task<PictureDto> MakePictureDto(Picture picture);
        Task<RatingDto> MakeRatingDto(FoodRating rating);
    }
}