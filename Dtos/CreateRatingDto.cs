using FoodRatingApi.Entities;

namespace FoodRatingApi.Dtos;

public record CreateRatingDto(Rating Rating, string PictureDataString);
