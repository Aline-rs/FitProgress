using FitProgress.Domain.Entities;

namespace FitProgress.Application.PhysicalRecords.Interfaces
{
    public interface IPhysicalRecordRepository
    {
        Task AddAsync(PhysicalRecord physicalRecord);
    }
}