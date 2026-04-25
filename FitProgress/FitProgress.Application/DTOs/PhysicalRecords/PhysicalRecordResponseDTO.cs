namespace FitProgress.Application.DTOs.PhysicalRecords
{
    public class PhysicalRecordResponseDTO
    {
        public Guid Id { get; set; }
        public DateOnly RecordDate { get; set; }
        public decimal Weight { get; set; }
        public string? Notes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}