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
public class PicturesController : ControllerBase
{
    private readonly FoodRatingDbContext _context;
    private readonly IStorageService _storageService;
    private readonly ILogger<PicturesController> _logger;

    public PicturesController(FoodRatingDbContext context, IStorageService storageService, ILogger<PicturesController> logger)
    {
        _context = context;
        _storageService = storageService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<NewPictureDto>> CreatePicture([FromBody] CreatePictureDto dto)
    {
        var userId = User.GetUserId();
        var picture = new Picture(dto.FileName, userId);
        var uploadUrl = await _storageService.GetPresignedUploadUrl(StorageBucketNames.Pictures, picture.ObjectName);
        await _context.Pictures.AddAsync(picture);
        await _context.SaveChangesAsync();
        return Ok(new NewPictureDto(picture.Id, uploadUrl));
    }

    [HttpDelete("{pictureId}")]
    public async Task<ActionResult> DeletePicture([FromRoute] Guid pictureId)
    {
        var userId = User.GetUserId();
        var picture = await _context.Pictures.FindAsync(pictureId);

        if (picture is null)
        {
            return NotFound();
        }

        // TODO: Check owner. Or store owner first :D

        await _storageService.DeleteObject(StorageBucketNames.Pictures, picture.ObjectName);
        _context.Remove(picture);
        await _context.SaveChangesAsync();
        return Ok();
    }
}