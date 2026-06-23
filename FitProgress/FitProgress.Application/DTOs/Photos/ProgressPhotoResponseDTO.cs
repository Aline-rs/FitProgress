namespace FitProgress.Application.DTOs.Photos
{
    public class ProgressPhotoResponseDTO
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}