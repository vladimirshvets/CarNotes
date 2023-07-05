using CarNotes.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarNotes.WebAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
public class ImagesController : ControllerBase
{
    private readonly IImageService _imageService;

    public ImagesController(
        IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpPost("avatar/{carId:guid}")]
    public async Task<ActionResult<string>> UploadCarAvatar(
        Guid carId, [FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Please select a file to upload.");
        }

        string fileExtension = Path.GetExtension(file.FileName);
        var acceptedExtensions = new List<string>
        {
            ".jpg"
        };
        if (!acceptedExtensions.Contains(fileExtension))
        {
            return BadRequest("Unsupported file extension.");
        }

        string? path = null;
        using (Stream stream = file.OpenReadStream())
        {
            path = await _imageService.SaveCarAvatarAsync(stream, fileExtension, carId);
        }

        if (path == null)
        {
            return BadRequest("Could not save file.");
        }

        return Ok(path);
    }
}
