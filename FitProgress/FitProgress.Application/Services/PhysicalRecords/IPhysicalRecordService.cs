using FitProgress.Application.Common;
using FitProgress.Application.DTOs.PhysicalRecords;

namespace FitProgress.Application.PhysicalRecords.Interfaces
{
    public interface IPhysicalRecordService
    {
        Task<ServiceResult<PhysicalRecordResponseDTO>> CreateAsync(Guid userId, CreatePhysicalRecordRequestDTO request);

        Task<ServiceResult<IEnumerable<PhysicalRecordResponseDTO>>> ListByUserAsync(Guid userId);

        Task<ServiceResult<PhysicalRecordResponseDTO>> UpdateAsync(
            
            Guid userId,
            Guid recordId,
            UpdatePhysicalRecordRequestDTO request);
        
        Task<ServiceResult<bool>> DeleteAsync(Guid userId, Guid recordId);
    }
}