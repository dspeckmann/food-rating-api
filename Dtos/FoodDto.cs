namespace FoodRatingApi.Dtos;

public record FoodDto(Guid Id, string Name, string? PictureDataString, DateTime CreatedAt, DateTime UpdatedAt);
