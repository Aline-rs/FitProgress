using FitProgress.Application.Common;
using FitProgress.Application.DTOs.PhysicalRecords;
using FitProgress.Application.PhysicalRecords.Interfaces;
using FitProgress.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Supabase.Gotrue;

namespace FitProgress.Application.Services.PhysicalRecords
{
    public class PhysicalRecordService : IPhysicalRecordService
    {
        private readonly IPhysicalRecordRepository _physicalRecordRepository;

        public PhysicalRecordService(IPhysicalRecordRepository physicalRecordRepository)
        {
            _physicalRecordRepository = physicalRecordRepository;
        }
        public async Task<ServiceResult<PhysicalRecordResponseDTO>> CreateAsync(Guid userId, CreatePhysicalRecordRequestDTO request)
        {
            if (request.Weight <= 0)
            {
                return ServiceResult<PhysicalRecordResponseDTO>.Fail(
                    "O peso deve ser maior que zero.");
            }

            var physicalRecord = new PhysicalRecord
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RecordDate = request.RecordDate,
                Weight = request.Weight,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow
            };
            await _physicalRecordRepository.AddAsync(physicalRecord);

            return ServiceResult<PhysicalRecordResponseDTO>.Ok(new PhysicalRecordResponseDTO
            {
                Id = physicalRecord.Id,
                RecordDate = physicalRecord.RecordDate,
                Weight = physicalRecord.Weight,
                Notes = physicalRecord.Notes,
                CreatedAt = physicalRecord.CreatedAt
            });
        }

        public async Task<ServiceResult<IEnumerable<PhysicalRecordResponseDTO>>> ListByUserAsync(Guid userId)

        {
            var records = await _physicalRecordRepository.GetByUserIdAsync(userId);
            var response = records.Select(record => new PhysicalRecordResponseDTO
            {
                Id = record.Id,
                RecordDate = record.RecordDate,
                Weight = record.Weight,
                Notes = record.Notes,
                CreatedAt = record.CreatedAt
            });
            return ServiceResult<IEnumerable<PhysicalRecordResponseDTO>>.Ok(response);
        }
    }
}
