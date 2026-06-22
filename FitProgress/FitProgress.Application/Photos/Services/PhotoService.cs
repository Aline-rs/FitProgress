using FitProgress.Application.Common;
using FitProgress.Application.DTOs.Photos;
using FitProgress.Application.Photos.Interfaces;
using FitProgress.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace FitProgress.Application.Photos.Services
{
    public class PhotoService : IPhotoService
    {
        private readonly ICloudinaryService _cloudinaryService;
        private readonly IProgressPhotoRepository _progressPhotoRepository;

        public PhotoService(
            ICloudinaryService cloudinaryService,
            IProgressPhotoRepository progressPhotoRepository)
        {
            _cloudinaryService = cloudinaryService;
            _progressPhotoRepository = progressPhotoRepository;
        }

        public async Task<ServiceResult<ProgressPhotoResponseDTO>> UploadAsync(
            Guid userId,
            Guid? physicalRecordId,
            IFormFile file)
        {
            if (file == null || file.Length == 0)
                return ServiceResult<ProgressPhotoResponseDTO>.Fail("Nenhuma imagem foi enviada.");

            var uploadResult = await _cloudinaryService.UploadPhotoAsync(file);

            if (!uploadResult.Success || uploadResult.Data == null)
                return ServiceResult<ProgressPhotoResponseDTO>.Fail(uploadResult.Message);

            var progressPhoto = new ProgressPhoto
            {
                UserId = userId,
                PhysicalRecordId = physicalRecordId,
                ImageUrl = uploadResult.Data.ImageUrl,
                PublicId = uploadResult.Data.PublicId,
                CreatedAt = DateTime.UtcNow
            };

            await _progressPhotoRepository.AddAsync(progressPhoto);

            var response = new ProgressPhotoResponseDTO
            {
                Id = progressPhoto.Id,
                ImageUrl = progressPhoto.ImageUrl,
                CreatedAt = progressPhoto.CreatedAt
            };

            return ServiceResult<ProgressPhotoResponseDTO>.Ok(response);
        }
    }
}