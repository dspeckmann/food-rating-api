using FoodRatingApi.Dtos;
using FoodRatingApi.Entities;
using FoodRatingApi.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Buffers.Text;
using System.Text;

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

    [HttpGet("/api/Pets/{petId}/[controller]/")]
    public async Task<ActionResult<IEnumerable<RatingDto>>> GetAllRatings([FromRoute] Guid petId)
    {
        var userId = User.GetUserId();
        var ratings = await _context.FoodRatings
            .Where(rating => rating.Food!.Pet!.OwnerIds.Contains(userId))
            .Select(rating => new RatingDto(rating.Rating, rating.Food!.Picture!.DataString, rating.CreatedAt))
            .ToListAsync();
        return Ok(ratings);
    }

    [HttpPost("/api/Pets/{petId}/[controller]/")]
    public async Task<ActionResult<RatingDto>> AddRating([FromRoute] Guid petId, [FromBody] CreateRatingDto dto)
    {
        var userId = User.GetUserId();
        var pet = await _context.FindAsync<Pet>(petId);
        if (pet is null)
        {
            return NotFound();
        }
        if (!pet.OwnerIds.Contains(userId))
        {
            return Forbid();
        }
        var picture = new Picture(dto.PictureDataString);
        var rating = new FoodRating(userId, dto.Rating);
        var food = new Food
        {
            Picture = picture,
            Pet = pet,
            FoodRatings = new List<FoodRating>() { rating }
        };
        _context.Add(food);
        await _context.SaveChangesAsync();
        return Ok(new RatingDto(rating.Rating, dto.PictureDataString, rating.CreatedAt));
    }
}