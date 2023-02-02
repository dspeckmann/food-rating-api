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
            .Include(rating => rating.Pet)
                .ThenInclude(pet => pet!.Picture);

        if (petId is not null)
        {
            query = query.Where(rating => rating.Pet!.Id == petId);
        }

        return await query
            .Select(rating => new RatingDto(
                new PetDto(rating.Pet!.Id, rating.Pet!.Name, rating.Pet!.Picture!.DataString, rating.Pet!.CreatedAt, rating.Pet!.UpdatedAt),
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
        var pet = await _context.Pets.FindAsync(dto.PetId);

        if (food is null || pet is null)
        {
            return BadRequest();
        }

        // TODO: Should foods also be shared across accounts?
        if (food.UserId != userId || !pet.OwnerIds.Contains(userId))
        {
            return Forbid();
        }

        var rating = new FoodRating(userId, pet, food, dto.Taste, dto.Wellbeing, dto.Comment);

        if (!string.IsNullOrWhiteSpace(dto.PictureId))
        {
            var picture = await _context.Pictures.FindAsync(dto.PictureId);
            if (picture is null)
            {
                return BadRequest();
            }
            rating.Picture = picture;
        }

        _context.Add(rating);
        await _context.SaveChangesAsync();
        return Ok(new RatingDto(
            new PetDto(rating.Pet!.Id, rating.Pet!.Name, rating.Pet!.Picture!.DataString, rating.Pet!.CreatedAt, rating.Pet!.UpdatedAt),
            new FoodDto(rating.Food!.Id, rating.Food!.Name, rating.Food!.Picture!.DataString, rating.Food.CreatedAt, rating.Food.UpdatedAt),
            rating.Taste,
            rating.Wellbeing,
            rating.Comment,
            rating.Picture!.DataString,
            rating.CreatedAt));
    }
}