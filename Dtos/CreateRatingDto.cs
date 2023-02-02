using FoodRatingApi.Entities;

namespace FoodRatingApi.Dtos;

public record CreateRatingDto(Guid FoodId, Guid PetId, Taste? Taste, Wellbeing? Wellbeing, string? Comment, Guid? PictureId);
