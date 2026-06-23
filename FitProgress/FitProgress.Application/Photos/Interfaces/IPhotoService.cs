using FitProgress.Application.Common;
using FitProgress.Application.DTOs.Photos;
using Microsoft.AspNetCore.Http;

namespace FitProgress.Application.Photos.Interfaces
{
    public interface IPhotoService
    {
        Task<ServiceResult<ProgressPhotoResponseDTO>> UploadAsync(
            Guid userId,
            Guid? physicalRecordId,
            IFormFile file);
    }
}