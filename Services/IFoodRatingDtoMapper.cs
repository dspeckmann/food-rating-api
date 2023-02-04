using FoodRatingApi.Dtos;
using FoodRatingApi.Entities;

namespace FoodRatingApi.Services
{
    public interface IFoodRatingDtoMapper
    {
        Task<FoodDto> MakeFoodDto(Food food);
        Task<PetDto> MakePetDto(Pet pet);
        Task<PictureDto> MakePictureDto(Picture picture);
        Task<RatingDto> MakeRatingDto(FoodRating rating);
    }
}