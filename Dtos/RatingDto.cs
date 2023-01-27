using FoodRatingApi.Entities;

namespace FoodRatingApi.Dtos;

public record RatingDto(
    IEnumerable<PetDto> Pets,
    FoodDto Food,
    Taste? Taste, 
    Wellbeing? Wellbeing, 
    string Comment, 
    string PictureDataString, 
    DateTime CreatedAt);
