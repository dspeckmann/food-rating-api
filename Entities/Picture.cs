namespace FoodRatingApi.Entities;

public class Picture
{
    public Guid Id { get; set; }
    public string DataString { get; set; }

    public Picture(string dataString) 
        : this(Guid.NewGuid(), dataString)
    {
    }

    public Picture(Guid id, string dataString)
    {
        Id = id;
        DataString = dataString;
    }
}
