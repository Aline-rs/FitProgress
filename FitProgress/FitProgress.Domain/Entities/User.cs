namespace FitProgress.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<PhysicalRecord> PhysicalRecords { get; set; } = new List<PhysicalRecord>();
        public ICollection<ProgressPhoto> ProgressPhotos { get; set; } = new List<ProgressPhoto>();

    }
}
