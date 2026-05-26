namespace FitProgress.Application.DTOs.PhysicalRecords
{
    public class CreatePhysicalRecordRequestDTO
    {
        public DateOnly RecordDate { get; set; }
        public decimal Weight { get; set; }
        public string? Notes { get; set; }
    }
}