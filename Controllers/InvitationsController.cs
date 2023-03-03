using FoodRatingApi.Dtos;
using FoodRatingApi.Entities;
using FoodRatingApi.Extensions;
using FoodRatingApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace FoodRatingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InvitationsController : ControllerBase
{
    private readonly TimeSpan _defaultInvitationValidity = TimeSpan.FromDays(7);
    private readonly FoodRatingDbContext _context;
    private readonly IStorageService _storageService;
    private readonly ILogger<InvitationsController> _logger;

    public InvitationsController(FoodRatingDbContext context, IStorageService storageService, ILogger<InvitationsController> logger)
    {
        _context = context;
        _storageService = storageService;
        _logger = logger;
    }

    [HttpPost("/api/pets/{petId}/[controller]")]
    public async Task<ActionResult<InvitationDto>> CreateInvitation([FromRoute] Guid petId)
    {
        var userId = User.GetUserId();
        var pet = await _context.Pets.FindAsync(petId);
        if (pet is null)
        {
            return NotFound();
        }

        var invitation = new Invitation(userId, pet, _defaultInvitationValidity);
        await _context.Invitations.AddAsync(invitation);
        await _context.SaveChangesAsync();

        return Ok(new InvitationDto(invitation.Id, invitation.ExpiresAt));
    }

    [HttpPost("{invitationId}/accept")]
    public async Task<ActionResult<PetDto>> AcceptInvitation([FromRoute] Guid invitationId)
    {
        var userId = User.GetUserId();
        var invitation = await _context.Invitations
            .Include(invitation => invitation.Pet)
                .ThenInclude(pet => pet!.Picture)
            .FirstOrDefaultAsync(invitation => invitation.Id == invitationId);

        if (invitation is null)
        {
            return NotFound();
        }

        var invalid = false;
        if (invitation.ExpiresAt < DateTime.UtcNow)
        {
            _logger.LogInformation($"Deleting invitation {invitationId} because it expired.");
            invalid = true;
        }
        else if (invitation.Pet is null)
        {
            _logger.LogInformation($"Deleting invitation {invitationId} because the pet does not exist anymore.");
            invalid = true;
        }
        else if (!invitation.Pet.OwnerIds.Contains(invitation.UserId))
        {
            _logger.LogInformation($"Deleting invitation {invitationId} because the inviting user does not own the pet anymore.");
            invalid = true;
        }
        else if (invitation.Pet.OwnerIds.Contains(userId))
        {
            // No need to delete the invitation since someone else might still use it.
            return NotFound();
        }

        if (invalid)
        {
            _context.Remove(invitation);
            await _context.SaveChangesAsync();
            return NotFound();
        }

        var pet = invitation.Pet!;
        pet.OwnerIds = pet.OwnerIds.Append(userId).ToArray();
        _context.Remove(invitation);
        await _context.SaveChangesAsync();

        return Ok(new PetDto(
            pet.Id,
            pet.Name,
            pet.Picture is not null
                ? new PictureDto(
                    pet.Picture.Id,
                    await _storageService.GetPresignedDownloadUrl(StorageBucketNames.Pictures, pet.Picture.ObjectName))
                : null,
            pet.CreatedAt,
            pet.UpdatedAt));
    }
}