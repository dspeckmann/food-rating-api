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
public class FoodsController : ControllerBase
{
    private readonly FoodRatingDbContext _context;
    private readonly ILogger<FoodsController> _logger;

    public FoodsController(FoodRatingDbContext context, ILogger<FoodsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FoodDto>>> GetAllFoods()
    {
        var userId = User.GetUserId();
        var foods = await _context.Foods
            .Include(food => food.Picture)
            .Where(food => food.UserId == userId)
            .Select(food => new FoodDto(food.Id, food.Name, food.Picture!.DataString, food.CreatedAt, food.UpdatedAt))
            .ToListAsync();
        return Ok(foods);
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

        return Ok(new FoodDto(food.Id, food.Name, food.Picture?.DataString, food.CreatedAt, food.UpdatedAt));
    }

    [HttpPost]
    public async Task<ActionResult<FoodDto>> CreateFood([FromBody] CreateFoodDto dto)
    {
        var userId = User.GetUserId();
        var food = new Food(userId, dto.Name, dto.Comment);

        if (!string.IsNullOrWhiteSpace(dto.PictureDataString))
        {
            food.Picture = new Picture(dto.PictureDataString);
        }

        _context.Add(food);
        await _context.SaveChangesAsync();
        return Ok(new FoodDto(food.Id, food.Name, food.Picture?.DataString, food.CreatedAt, food.UpdatedAt));
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
        if (!string.IsNullOrWhiteSpace(dto.PictureDataString))
        {
            if (food.Picture is not null)
            {
                food.Picture.DataString = dto.PictureDataString;
            }
            else
            {
                food.Picture = new Picture(dto.PictureDataString);
            }
        }

        await _context.SaveChangesAsync();
        return Ok(new FoodDto(food.Id, food.Name, food.Picture?.DataString, food.CreatedAt, food.UpdatedAt));
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
}