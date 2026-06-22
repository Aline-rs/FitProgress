using FitProgress.Application.DTOs.Photos;
using FitProgress.Application.Photos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitProgress.Api.Controllers
{
    [ApiController]
    [Route("api/photos")]
    [Authorize]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotosController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] UploadPhotoRequestDTO request)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!Guid.TryParse(userIdClaim, out var userId))
                return Unauthorized("Usuário inválido.");

            var result = await _photoService.UploadAsync(
                 
                userId,
                request.PhysicalRecordId,
                request.File);

            if (!result.Success)
                return BadRequest(result.Message);

            return Created("", result.Data);
        }
    }
}