namespace FoodRatingApi.Entities;

public class Picture
{
    public Guid Id { get; set; }
    public string OriginalFileName { get; set; }
    public string ObjectName { get; set; }

    public Picture(Guid id, string originalFileName, string objectName) 
    {
        Id = id;
        OriginalFileName = originalFileName;
        ObjectName = objectName;
    }

    public Picture(string originalFileName, string userId)
    {
        Id = Guid.NewGuid();
        OriginalFileName = originalFileName;
        var extension = Path.GetExtension(originalFileName);
        ObjectName = $"{userId}/{Id}{extension}";
    }
}
