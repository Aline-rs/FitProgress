using FitProgress.Application.Common;
using FitProgress.Application.DTOs.PhysicalRecords;

namespace FitProgress.Application.PhysicalRecords.Interfaces
{
    public interface IPhysicalRecordService
    {
        Task<ServiceResult<PhysicalRecordResponseDTO>> CreateAsync(Guid userId, CreatePhysicalRecordRequestDTO request);

        Task<ServiceResult<IEnumerable<PhysicalRecordResponseDTO>>> ListByUserAsync(Guid userId);
    }
}