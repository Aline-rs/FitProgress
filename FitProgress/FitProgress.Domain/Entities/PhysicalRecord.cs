namespace FitProgress.Domain.Entities
{
    public class PhysicalRecord
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime RecordDate { get; set; }
        public decimal Weight { get; set; }
        public string? Notes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public User User { get; set; } = null!;
    }
}
