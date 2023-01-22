namespace FoodRatingApi.Entities;

public class Pet
{
    public Guid Id { get; set; }
    public string[] OwnerIds { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; }
    public ICollection<Food> Foods { get; set; }

    public Pet(string name)
        : this(Guid.NewGuid(), Array.Empty<string>(), DateTime.UtcNow, name, new List<Food>())
    {
    }

    public Pet(Guid id, string[] ownerIds, DateTime createdAt, string name, ICollection<Food> foods)
    {
        Id = id;
        OwnerIds = ownerIds;
        CreatedAt = createdAt;
        Name = name;
        Foods = foods;
    }
}
