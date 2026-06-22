using Microsoft.AspNetCore.Http;

namespace FitProgress.Application.DTOs.Photos
{
    public class UploadPhotoRequestDTO
    {
        public IFormFile File { get; set; } = null!;
        public Guid? PhysicalRecordId { get; set; }
    }
}
