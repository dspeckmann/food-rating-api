namespace FoodRatingApi.Entities;

public class Invitation
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public Pet? Pet { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }

    public Invitation(string userId, Pet? pet, TimeSpan validity)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Pet = pet;
        CreatedAt = DateTime.UtcNow;
        ExpiresAt = CreatedAt.Add(validity);
    }

    public Invitation(Guid id, string userId, Pet? pet, DateTime createdAt, DateTime expiresAt)
    {
        Id = id;
        UserId = userId;
        Pet = pet;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
    }
}
