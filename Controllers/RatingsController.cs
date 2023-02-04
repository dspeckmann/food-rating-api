using FoodRatingApi.Dtos;
using FoodRatingApi.Entities;
using FoodRatingApi.Extensions;
using FoodRatingApi.Services;
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
    private readonly IStorageService _storageService;
    private readonly IFoodRatingDtoMapper _mapper;
    private readonly ILogger<RatingsController> _logger;

    public RatingsController(
        FoodRatingDbContext context,
        IStorageService storageService,
        IFoodRatingDtoMapper mapper,
        ILogger<RatingsController> logger)
    {
        _context = context;
        _storageService = storageService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet("/api/[controller]/")]
    public async Task<ActionResult<IEnumerable<RatingDto>>> GetAllRatings()
    {
        var ratings = await GetRatingsAsync();
        return Ok(ratings);
    }

    [HttpGet("/api/pets/{petId}/[controller]/")]
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

        var ratings = await query.ToListAsync();
        return await Task.WhenAll(ratings.Select(rating => _mapper.MakeRatingDto(rating)));

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

        if (dto.PictureId is not null)
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
        return Ok(await _mapper.MakeRatingDto(rating));
    }

    [HttpDelete("{ratingId}")]
    public async Task<ActionResult> DeleteRating([FromRoute] Guid ratingId)
    {
        var userId = User.GetUserId();
        var rating = await _context.FoodRatings
            .Include(rating => rating.Pet)
            .FirstOrDefaultAsync(rating => rating.Id == ratingId);
        
        if (rating is null)
        {
            return NotFound();
        }
        
        if (!rating.Pet!.OwnerIds.Contains(userId))
        {
            return Forbid();
        }

        var pictureObjectName = rating.Picture?.ObjectName;
        _context.Remove(rating);
        await _context.SaveChangesAsync();

        if (!string.IsNullOrWhiteSpace(pictureObjectName))
        {
            await _storageService.DeleteObject(StorageBucketNames.Pictures, pictureObjectName);
        }

        return Ok();
    }
}