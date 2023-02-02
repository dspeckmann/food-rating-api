using FoodRatingApi.Entities;

namespace FoodRatingApi.Dtos;

public record RatingDto(
    PetDto Pet,
    FoodDto Food,
    Taste? Taste, 
    Wellbeing? Wellbeing, 
    string Comment,
    PictureDto? Picture,
    DateTime CreatedAt);
