using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FitProgress.Application.Common;
using FitProgress.Application.DTOs.Photos;
using FitProgress.Application.Photos.Interfaces;
using FitProgress.Application.Settings;
using FitProgress.Infrastructure.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace FitProgress.Infrastructure.Photos.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> cloudinarySettings)
        {
            var account = new Account(
                cloudinarySettings.Value.CloudName,
                cloudinarySettings.Value.ApiKey,
                cloudinarySettings.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(account);
        }

        public async Task<ServiceResult<CloudinaryUploadResponseDTO>> UploadPhotoAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return ServiceResult<CloudinaryUploadResponseDTO>.Fail("Nenhuma imagem foi enviada.");

            await using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "fitprogress/photos"
            };

            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            if (uploadResult.Error != null)
                return ServiceResult<CloudinaryUploadResponseDTO>.Fail(uploadResult.Error.Message);

            var response = new CloudinaryUploadResponseDTO
            {
                ImageUrl = uploadResult.SecureUrl.ToString(),
                PublicId = uploadResult.PublicId
            };

            return ServiceResult<CloudinaryUploadResponseDTO>.Ok(response);
        }
    }
}
