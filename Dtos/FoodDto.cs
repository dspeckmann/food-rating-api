namespace FoodRatingApi.Dtos;

public record FoodDto(Guid Id, string Name, PictureDto? Picture, DateTime CreatedAt, DateTime UpdatedAt, bool? IsRatedWell = null, DateTime? LastRatingDate = null);
