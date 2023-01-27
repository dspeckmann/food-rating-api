namespace FoodRatingApi.Dtos;

public record PetDto(Guid Id, string Name, string? PictureDataString, DateTime CreatedAt, DateTime UpdatedAt);
