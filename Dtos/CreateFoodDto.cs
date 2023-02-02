namespace FoodRatingApi.Dtos;

public record CreateFoodDto(string Name = "", string Comment = "", Guid? PictureId = null);
