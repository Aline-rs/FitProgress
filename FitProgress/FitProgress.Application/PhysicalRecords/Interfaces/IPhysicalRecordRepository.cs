using FitProgress.Domain.Entities;

namespace FitProgress.Application.PhysicalRecords.Interfaces
{
    public interface IPhysicalRecordRepository
    {
        Task AddAsync(PhysicalRecord physicalRecord);

        Task<IEnumerable<PhysicalRecord>> GetByUserIdAsync(Guid userId);

        Task<IEnumerable<PhysicalRecord>> GetByUserIdAsync(Guid userId);
        
        Task<PhysicalRecord?> GetByIdAndUserIdAsync(Guid recordId, Guid userId);

        Task UpdateAsync(PhysicalRecord physicalRecord);

        Task DeleteAsync(PhysicalRecord physicalRecord);
    }
}

