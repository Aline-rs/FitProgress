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
                CreatedAt = physicalRecord.CreatedAt.AddHours(-3) // Ajuste para horário de Brasília (UTC-3)
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
                CreatedAt = record.CreatedAt.AddHours(-3) // Ajuste para horário de Brasília (UTC-3)
            });
            return ServiceResult<IEnumerable<PhysicalRecordResponseDTO>>.Ok(response);
        }

        public async Task<ServiceResult<PhysicalRecordResponseDTO>> UpdateAsync(
            Guid userId,
            Guid recordId,
            UpdatePhysicalRecordRequestDTO request)
        
        {
            if (request.Weight <= 0)
            {
                return ServiceResult<PhysicalRecordResponseDTO>.Fail(
                        "O peso deve ser maior que zero.");
            }

            var physicalRecord = await _physicalRecordRepository
                .GetByIdAndUserIdAsync(recordId, userId);
           
            if (physicalRecord == null)
            {
                return ServiceResult<PhysicalRecordResponseDTO>.Fail(
                    "Registro físico não encontrado ou não pertence ao usuário.");
            }

            physicalRecord.RecordDate = request.RecordDate;
            physicalRecord.Weight = request.Weight;
            physicalRecord.Notes = request.Notes;

            await _physicalRecordRepository.UpdateAsync(physicalRecord);


            return ServiceResult<PhysicalRecordResponseDTO>.Ok(new PhysicalRecordResponseDTO
            {
                Id = physicalRecord.Id,
                RecordDate = physicalRecord.RecordDate,
                Weight = physicalRecord.Weight,
                Notes = physicalRecord.Notes,
                CreatedAt = physicalRecord.CreatedAt.AddHours(-3) // Ajuste para horário de Brasília (UTC-3)
            });
        }

        public async Task<ServiceResult<bool>> DeleteAsync(Guid userId, Guid recordId)
        {

            var physicalRecord = await _physicalRecordRepository
                .GetByIdAndUserIdAsync(recordId, userId);

            if (physicalRecord == null)
            {
                return ServiceResult<bool>.Fail("Registro físico não encontrado ou não pertence ao usuário.");
            }

            await _physicalRecordRepository.DeleteAsync(physicalRecord);

            return ServiceResult<bool>.Ok(true);

        }
    }
}
