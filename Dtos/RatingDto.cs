using FoodRatingApi.Entities;

namespace FoodRatingApi.Dtos;

public record RatingDto(Rating Rating, string PictureDataString, DateTime CreatedAt);
