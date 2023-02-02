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
public class FoodsController : ControllerBase
{
    private readonly FoodRatingDbContext _context;
    private readonly IStorageService _storageService;
    private readonly ILogger<FoodsController> _logger;

    public FoodsController(FoodRatingDbContext context, IStorageService storageService ,ILogger<FoodsController> logger)
    {
        _context = context;
        _storageService = storageService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FoodDto>>> GetAllFoods()
    {
        var userId = User.GetUserId();
        var foods = await _context.Foods
            .Include(food => food.Picture)
            .Where(food => food.UserId == userId)
            .ToListAsync();

        var dtos = foods.Select(async food => await MakeFoodDto(food));

        return Ok(dtos);
    }

    [HttpGet("{foodId}")]
    public async Task<ActionResult<FoodDto>> GetFoodById(Guid foodId)
    {
        var userId = User.GetUserId();
        var food = await _context.Foods
            .Include(food => food.Picture)
            .FirstOrDefaultAsync(food => food.Id == foodId);

        if (food is null)
        {
            return NotFound();
        }

        if (food.UserId != userId)
        {
            return Forbid();
        }

        return Ok(await MakeFoodDto(food));
    }

    [HttpPost]
    public async Task<ActionResult<FoodDto>> CreateFood([FromBody] CreateFoodDto dto)
    {
        var userId = User.GetUserId();
        var food = new Food(userId, dto.Name, dto.Comment);

        if (dto.PictureId is not null)
        {
            var picture = await _context.Pictures.FindAsync(dto.PictureId);
            if (picture is null)
            {
                return BadRequest();
            }

            food.Picture = picture;
        }

        _context.Add(food);
        await _context.SaveChangesAsync();
        return Ok(await MakeFoodDto(food));
    }

    [HttpPut("{foodId}")]
    public async Task<ActionResult<FoodDto>> UpdateFood([FromRoute] Guid foodId, [FromBody] CreateFoodDto dto)
    {
        var userId = User.GetUserId();
        var food = await _context.Foods
            .Include(food => food.Picture)
            .FirstOrDefaultAsync(food => food.Id == foodId);

        if (food is null)
        {
            return NotFound();
        }

        if (food.UserId != userId)
        {
            return Forbid();
        }

        food.Name = dto.Name;
        food.Comment = dto.Comment;
        food.UpdatedAt = DateTime.UtcNow;

        var oldPictureObjectName = string.Empty;
        if (dto.PictureId is not null && (food.Picture is null || food.Picture.Id != dto.PictureId))
        {
            if (food.Picture is not null)
            {
                oldPictureObjectName = food.Picture.ObjectName;
                _context.Pictures.Remove(food.Picture);
            }

            var picture = await _context.Pictures.FindAsync(dto.PictureId);
            if (picture is null)
            {
                return BadRequest();
            }

            food.Picture = picture;
        }

        await _context.SaveChangesAsync();
        if (!string.IsNullOrWhiteSpace(oldPictureObjectName))
        {
            await _storageService.DeleteObject(StorageBucketNames.Pictures, oldPictureObjectName);
        }

        return Ok(await MakeFoodDto(food));
    }

    [HttpDelete("{foodId}")]
    public async Task<ActionResult> DeleteFood([FromRoute] Guid foodId)
    {
        var userId = User.GetUserId();
        var food = await _context.FindAsync<Food>(foodId);

        if (food is null)
        {
            return NotFound();
        }

        if (food.UserId != userId)
        {
            return Forbid();
        }

        _context.Remove(food);
        await _context.SaveChangesAsync();
        return Ok();
    }

    private async Task<FoodDto> MakeFoodDto(Food food)
    {
        var pictureDto = food.Picture is not null
            ? new PictureDto(food.Picture.Id, await _storageService.GetPresignedDownloadUrl(StorageBucketNames.Pictures, food.Picture.ObjectName)
            : null;

        return new FoodDto(
            food.Id,
            food.Name,
            pictureDto,
            food.CreatedAt,
            food.UpdatedAt);
    }
}