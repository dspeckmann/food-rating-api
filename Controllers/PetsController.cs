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
public class PetsController : ControllerBase
{
    private readonly FoodRatingDbContext _context;
    private readonly ILogger<PetsController> _logger;

    public PetsController(FoodRatingDbContext context, ILogger<PetsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PetDto>>> GetAllPets()
    {
        var userId = User.GetUserId();
        var pets = await _context.Pets
            .Include(pet => pet.Picture)
            .Where(pet => pet.OwnerIds.Contains(userId))
            .Select(pet => new PetDto(pet.Id, pet.Name, pet.Picture!.DataString, pet.CreatedAt, pet.UpdatedAt))
            .ToListAsync();
        return Ok(pets);
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

        return Ok(new PetDto(pet.Id, pet.Name, pet.Picture?.DataString, pet.CreatedAt, pet.UpdatedAt));
    }

    [HttpPost]
    public async Task<ActionResult<PetDto>> CreatePet([FromBody] CreatePetDto dto)
    {
        var userId = User.GetUserId();
        var pet = new Pet(dto.Name)
        {
            OwnerIds = new[] { userId }
        };

        if (!string.IsNullOrWhiteSpace(dto.PictureDataString))
        {
            pet.Picture = new Picture(dto.PictureDataString);
        }

        _context.Add(pet);
        await _context.SaveChangesAsync();
        return Ok(new PetDto(pet.Id, pet.Name, pet.Picture?.DataString, pet.CreatedAt, pet.UpdatedAt));
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
        if (!string.IsNullOrWhiteSpace(dto.PictureDataString))
        {
            if (pet.Picture is not null)
            {
                pet.Picture.DataString = dto.PictureDataString;
            }
            else
            {
                pet.Picture = new Picture(dto.PictureDataString);
            }
        }

        await _context.SaveChangesAsync();
        return Ok(new PetDto(pet.Id, pet.Name, pet.Picture?.DataString, pet.CreatedAt, pet.UpdatedAt));
    }

    [HttpDelete("{petId}")]
    public async Task<ActionResult> DeletePet([FromRoute] Guid petId)
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
        _context.Remove(pet);
        await _context.SaveChangesAsync();
        return Ok();
    }
}