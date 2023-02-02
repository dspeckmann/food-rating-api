namespace FoodRatingApi.Dtos;

public record PetDto(Guid Id, string Name, PictureDto? Picture, DateTime CreatedAt, DateTime UpdatedAt);
