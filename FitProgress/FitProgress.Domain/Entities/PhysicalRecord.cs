namespace FitProgress.Domain.Entities
{
    public class PhysicalRecord
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateOnly RecordDate { get; set; }
        public decimal Weight { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public User User { get; set; } = null!;
        public ICollection<ProgressPhoto> ProgressPhotos { get; set; } = new List<ProgressPhoto>();
    }
}
