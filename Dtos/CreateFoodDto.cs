namespace FoodRatingApi.Dtos;

public record CreateFoodDto(string Name = "", string Comment = "", string? PictureDataString = null);
