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
public class PetsController : ControllerBase
{
    private readonly FoodRatingDbContext _context;
    private readonly IStorageService _storageService;
    private readonly IFoodRatingDtoMapper _mapper;
    private readonly ILogger<PetsController> _logger;

    public PetsController(
        FoodRatingDbContext context,
        IStorageService storageService,
        IFoodRatingDtoMapper mapper,
        ILogger<PetsController> logger)
    {
        _context = context;
        _storageService = storageService;
        _mapper = mapper;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PetDto>>> GetAllPets()
    {
        var userId = User.GetUserId();
        var pets = await _context.Pets
            .Include(pet => pet.Picture)
            .Where(pet => pet.OwnerIds.Contains(userId))
            .ToListAsync();

        var dtos = await Task.WhenAll(pets.Select(pet => _mapper.MakePetDto(pet)));
        return Ok(dtos);
    }

    [HttpGet("{petId}")]
    public async Task<ActionResult<PetDto>> GetPetById(Guid petId)
    {
        var userId = User.GetUserId();
        var pet = await _context.Pets
            .Include(pet => pet.Picture)
            .FirstOrDefaultAsync(pet => pet.Id == petId);

        if (pet is null)
        {
            return NotFound();
        }

        if (!pet.OwnerIds.Contains(userId))
        {
            return Forbid();
        }

        return Ok(await _mapper.MakePetDto(pet));
    }

    [HttpPost]
    public async Task<ActionResult<PetDto>> CreatePet([FromBody] CreatePetDto dto)
    {
        var userId = User.GetUserId();
        var pet = new Pet(dto.Name)
        {
            OwnerIds = new[] { userId }
        };

        if (dto.PictureId is not null)
        {
            var picture = await _context.Pictures.FindAsync(dto.PictureId);
            if (picture is null)
            {
                return BadRequest();
            }

            pet.Picture = picture;
        }

        _context.Add(pet);
        await _context.SaveChangesAsync();
        return Ok(await _mapper.MakePetDto(pet));
    }

    [HttpPut("{petId}")]
    public async Task<ActionResult<PetDto>> UpdatePet([FromRoute] Guid petId, [FromBody] CreatePetDto dto)
    {
        var userId = User.GetUserId();
        var pet = await _context.Pets
            .Include(pet => pet.Picture)
            .FirstOrDefaultAsync(pet => pet.Id == petId);

        if (pet is null)
        {
            return NotFound();
        }

        if (!pet.OwnerIds.Contains(userId))
        {
            return Forbid();
        }

        pet.Name = dto.Name;
        pet.UpdatedAt = DateTime.UtcNow;

        var oldPictureObjectName = string.Empty;
        if (dto.PictureId is not null && (pet.Picture is null || pet.Picture.Id != dto.PictureId))
        {
            if (pet.Picture is not null)
            {
                oldPictureObjectName = pet.Picture.ObjectName;
                _context.Pictures.Remove(pet.Picture);
            }

            var picture = await _context.Pictures.FindAsync(dto.PictureId);
            if (picture is null)
            {
                return BadRequest();
            }

            pet.Picture = picture;
        }

        await _context.SaveChangesAsync();
        if (!string.IsNullOrWhiteSpace(oldPictureObjectName))
        {
            await _storageService.DeleteObject(StorageBucketNames.Pictures, oldPictureObjectName);
        }

        return Ok(await _mapper.MakePetDto(pet));
    }

    [HttpDelete("{petId}")]
    public async Task<ActionResult> DeletePet([FromRoute] Guid petId)
    {
        var userId = User.GetUserId();
        var pet = await _context.Pets
            .Include(pet => pet.Picture)
            .FirstOrDefaultAsync(pet => pet.Id == petId);

        if (pet is null)
        {
            return NotFound();
        }

        if (!pet.OwnerIds.Contains(userId))
        {
            return Forbid();
        }

        var pictureObjectName = pet.Picture?.ObjectName;
        _context.Remove(pet);
        await _context.SaveChangesAsync();

        if (!string.IsNullOrWhiteSpace(pictureObjectName))
        {
            await _storageService.DeleteObject(StorageBucketNames.Pictures, pictureObjectName);
        }

        return Ok();
    }
}