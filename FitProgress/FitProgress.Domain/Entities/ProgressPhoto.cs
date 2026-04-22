namespace FitProgress.Domain.Entities
{
    public class ProgressPhoto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid? PhysicalRecordId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? PublicId { get; set; }
        public DateTime CreatedAt { get; set; }
        public User User { get; set; } = null!;
        public PhysicalRecord? PhysicalRecord { get; set; } = null!;
    }
}
