namespace FoodRatingApi.Entities;

public class Food
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public Picture? Picture { get; set; }
    public Pet? Pet { get; set; }
    public ICollection<FoodRating> FoodRatings { get; set; }

    public Food()
        : this(Guid.NewGuid(), DateTime.UtcNow, null, null, new List<FoodRating>())
    {
    }

    public Food(Picture picture, Pet pet)
        : this(Guid.NewGuid(), DateTime.UtcNow, picture, pet, new List<FoodRating>())
    {
    }

    public Food(Guid id, DateTime createdAt, Picture? picture, Pet? pet, ICollection<FoodRating> foodRatings)
    {
        Id = id;
        CreatedAt = createdAt;
        Picture = picture;
        Pet = pet;
        FoodRatings = foodRatings;
    }
}
