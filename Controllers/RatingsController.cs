using FoodRatingApi.Dtos;
using FoodRatingApi.Entities;
using FoodRatingApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FoodRatingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class RatingsController : ControllerBase
{
    private readonly FoodRatingDbContext _context;
    private readonly ILogger<RatingsController> _logger;

    public RatingsController(FoodRatingDbContext context, ILogger<RatingsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("/api/[controller]/")]
    public async Task<ActionResult<IEnumerable<RatingDto>>> GetAllRatings()
    {
        var ratings = await GetRatingsAsync();
        return Ok(ratings);
    }

    [HttpGet("/api/Pets/{petId}/[controller]/")]
    public async Task<ActionResult<IEnumerable<RatingDto>>> GetRatingsByPetId([FromRoute] Guid petId)
    {
        var ratings = await GetRatingsAsync(petId);
        return Ok(ratings);
    }

    private async Task<IEnumerable<RatingDto>> GetRatingsAsync(Guid? petId = null)
    {
        var userId = User.GetUserId();
        IQueryable<FoodRating> query = _context.FoodRatings
            .Where(rating => rating.UserId == userId)
            .Include(rating => rating.Picture)
            .Include(rating => rating.Food)
                .ThenInclude(food => food!.Picture)
            .Include(rating => rating.Pets)
                .ThenInclude(pet => pet.Picture);

        if (petId is not null)
        {
            query = query.Where(rating => rating.Pets.Any(pet => pet.Id == petId));
        }

        return await query
            .Select(rating => new RatingDto(
                rating.Pets.Select(pet => new PetDto(pet.Id, pet.Name, pet.Picture!.DataString, pet.CreatedAt, pet.UpdatedAt)),
                new FoodDto(rating.Food!.Id, rating.Food!.Name, rating.Food!.Picture!.DataString, rating.Food.CreatedAt, rating.Food.UpdatedAt),
                rating.Taste,
                rating.Wellbeing,
                rating.Comment,
                rating.Picture!.DataString,
                rating.CreatedAt))
            .ToListAsync();
    }

    [HttpPost("/api/[controller]/")]
    public async Task<ActionResult<RatingDto>> AddRating([FromBody] CreateRatingDto dto)
    {
        var userId = User.GetUserId();
        var food = await _context.Foods.FindAsync(dto.FoodId);
        var pets = await _context.Pets
            .Where(pet => dto.PetIds.Contains(pet.Id))
            .ToListAsync();

        if (food is null || pets.Count != dto.PetIds.Length)
        {
            return BadRequest();
        }

        // TODO: Should foods also be shared across accounts?
        if (food.UserId != userId || pets.Any(pet => !pet.OwnerIds.Contains(userId)))
        {
            return Forbid();
        }

        var rating = new FoodRating(userId, pets, food, dto.Taste, dto.Wellbeing, dto.Comment);

        if (!string.IsNullOrWhiteSpace(dto.PictureDataString))
        {
            rating.Picture = new Picture(dto.PictureDataString);
        }

        _context.Add(rating);
        await _context.SaveChangesAsync();
        return Ok(new RatingDto(
            rating.Pets.Select(pet => new PetDto(pet.Id, pet.Name, pet.Picture!.DataString, pet.CreatedAt, pet.UpdatedAt)),
            new FoodDto(rating.Food!.Id, rating.Food!.Name, rating.Food!.Picture!.DataString, rating.Food.CreatedAt, rating.Food.UpdatedAt),
            rating.Taste,
            rating.Wellbeing,
            rating.Comment,
            rating.Picture!.DataString,
            rating.CreatedAt));
    }
}