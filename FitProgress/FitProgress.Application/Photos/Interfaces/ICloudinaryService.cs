using FitProgress.Application.Common;
using FitProgress.Application.DTOs.Photos;
using Microsoft.AspNetCore.Http;

namespace FitProgress.Application.Photos.Interfaces
{
    public interface ICloudinaryService
    {
        Task<ServiceResult<CloudinaryUploadResponseDTO>> UploadPhotoAsync(IFormFile file);
    }
}
