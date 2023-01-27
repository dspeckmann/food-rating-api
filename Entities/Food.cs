namespace FoodRatingApi.Entities;

public class Food
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string Name { get; set; }
    public string Comment { get; set; }
    public Picture? Picture { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<FoodRating> FoodRatings { get; set; }

    public Food(string userId, string? name = null, string? comment = null)
        : this(Guid.NewGuid(), userId, name, comment, null, DateTime.UtcNow, DateTime.UtcNow, new List<FoodRating>())
    {
    }

    public Food(Guid id, string userId, string? name, string? comment, Picture? picture, DateTime createdAt, DateTime updatedAt, ICollection<FoodRating> foodRatings)
    {
        Id = id;
        UserId = userId;
        Name = name ?? string.Empty;
        Comment = comment ?? string.Empty;
        Picture = picture;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        FoodRatings = foodRatings;

    }
}
