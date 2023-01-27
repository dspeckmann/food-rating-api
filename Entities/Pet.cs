namespace FoodRatingApi.Entities;

public class Pet
{
    public Guid Id { get; set; }
    public string[] OwnerIds { get; set; }
    public string Name { get; set; }
    public Picture? Picture { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public ICollection<FoodRating> FoodRatings { get; set; }

    public Pet(string name)
        : this(Guid.NewGuid(), Array.Empty<string>(), name, null, DateTime.UtcNow, DateTime.UtcNow, new List<FoodRating>())
    {
    }

    public Pet(Guid id, string[] ownerIds, string name, Picture? picture, DateTime createdAt, DateTime updatedAt, ICollection<FoodRating> foodRatings)
    {
        Id = id;
        OwnerIds = ownerIds;
        Name = name;
        Picture = picture;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        FoodRatings = foodRatings;
    }
}
