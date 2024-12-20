﻿namespace FoodRatingApi.Entities;

public class FoodRating
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public Pet? Pet { get; set; }
    public Food? Food { get; set; }
    public Taste? Taste { get; set; }
    public Wellbeing? Wellbeing { get; set; }
    public string Comment { get; set; }
    public Picture? Picture { get; set; }
    public DateTime CreatedAt { get; set; }

    public FoodRating(string userId, string? comment = null)
        : this(userId, null, null, null, null, comment, null, DateTime.UtcNow)
    {
    }

    public FoodRating(string userId, Pet? pet, Food food, Taste? taste, Wellbeing? wellbeing, string? comment)
        : this(userId, pet, food, taste, wellbeing, comment, null, DateTime.UtcNow)
    {
    }

    public FoodRating(string userId, Pet? pet, Food? food, Taste? taste, Wellbeing? wellbeing, string? comment, Picture? picture, DateTime createdAt)
    {
        UserId = userId;
        Pet = pet;
        Food = food;
        Taste = taste;
        Wellbeing = wellbeing;
        Comment = comment ?? string.Empty;
        CreatedAt = createdAt;
    }
}
