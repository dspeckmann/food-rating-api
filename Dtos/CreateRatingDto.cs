using FoodRatingApi.Entities;

namespace FoodRatingApi.Dtos;

public record CreateRatingDto(Guid FoodId, Guid[] PetIds, Taste? Taste, Wellbeing? Wellbeing, string? Comment, string? PictureDataString);
