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
    private readonly IFoodRatingDtoMapper _mapper;
    private readonly ILogger<FoodsController> _logger;

    public FoodsController(
        FoodRatingDbContext context,
        IStorageService storageService,
        IFoodRatingDtoMapper mapper,
        ILogger<FoodsController> logger)
    {
        _context = context;
        _storageService = storageService;
        _mapper = mapper;
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

        var dtos = await Task.WhenAll(foods.Select(food => _mapper.MakeFoodDto(food)));
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

        return Ok(await _mapper.MakeFoodDto(food));
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
        return Ok(await _mapper.MakeFoodDto(food));
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

        return Ok(await _mapper.MakeFoodDto(food));
    }

    [HttpDelete("{foodId}")]
    public async Task<ActionResult> DeleteFood([FromRoute] Guid foodId)
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

        var pictureObjectName = food.Picture?.ObjectName;
        _context.Remove(food);
        await _context.SaveChangesAsync();

        if (!string.IsNullOrWhiteSpace(pictureObjectName))
        {
            await _storageService.DeleteObject(StorageBucketNames.Pictures, pictureObjectName);
        }

        return Ok();
    }
}