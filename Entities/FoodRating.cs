namespace FoodRatingApi.Entities;

public class FoodRating
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public Food? Food { get; set; }
    public Rating Rating { get; set; }
    public DateTime CreatedAt { get; set; }

    public FoodRating(string userId, Rating rating)
        : this(userId, null, rating, DateTime.UtcNow)
    {
    }

    public FoodRating(string userId, Food food, Rating rating)
        : this(userId, food, rating, DateTime.UtcNow)
    {
    }

    public FoodRating(string userId, Food? food, Rating rating, DateTime createdAt)
    {
        UserId = userId;
        Food = food;
        Rating = rating;
        CreatedAt = createdAt;
    }
}
