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
            .Where(pet => pet.OwnerIds.Contains(userId))
            .Select(pet => new PetDto(pet.Id, pet.Name))
            .ToListAsync();
        return Ok(pets);
    }

    [HttpGet("{petId}")]
    public async Task<ActionResult<PetDto>> GetPetById(Guid petId)
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
        return new PetDto(pet.Id, pet.Name);
    }

    [HttpPost]
    public async Task<ActionResult<PetDto>> CreatePet([FromBody] CreatePetDto dto)
    {
        var userId = User.GetUserId();
        var pet = new Pet(dto.Name)
        {
            OwnerIds = new[] { userId }
        };
        _context.Add(pet);
        await _context.SaveChangesAsync();
        return Ok(new PetDto(pet.Id, pet.Name));
    }

    [HttpPut("{petId}")]
    public async Task<ActionResult<PetDto>> UpdatePet([FromRoute] Guid petId, [FromBody] CreatePetDto dto)
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
        pet.Name = dto.Name;
        await _context.SaveChangesAsync();
        return Ok(new PetDto(pet.Id, pet.Name));
    }

    [HttpDelete("{petId}")]
    public async Task<ActionResult<PetDto>> DeletePet([FromRoute] Guid petId)
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
        return Ok(new PetDto(pet.Id, pet.Name));
    }
}