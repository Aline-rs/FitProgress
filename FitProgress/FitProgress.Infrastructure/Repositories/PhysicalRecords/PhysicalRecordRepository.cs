
using Microsoft.EntityFrameworkCore;
using FitProgress.Application.PhysicalRecords.Interfaces;
using FitProgress.Domain.Entities;
using FitProgress.Infrastructure.Data;

namespace FitProgress.Infrastructure.Repositories.PhysicalRecords
{
    public class PhysicalRecordRepository : IPhysicalRecordRepository
    {
        private readonly AppDbContext _context;

        public PhysicalRecordRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(PhysicalRecord physicalRecord)
        {
            _context.PhysicalRecords.Add(physicalRecord);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PhysicalRecord>> GetByUserIdAsync(Guid userId)
        {
            return await _context.PhysicalRecords
                   .Where(r => r.UserId == userId)
                   .OrderByDescending(r => r.RecordDate)
                   .ToListAsync();
        }
        
        public async Task<PhysicalRecord?> GetByIdAndUserIdAsync(Guid recordId, Guid userId)
        {
            return await _context.PhysicalRecords
                .FirstOrDefaultAsync(r => r.Id == recordId && r.UserId == userId);
        }

        public async Task DeleteAsync(PhysicalRecord physicalRecord)
        {
            _context.PhysicalRecords.Remove(physicalRecord);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(PhysicalRecord physicalRecord)
        {
            _context.PhysicalRecords.Update(physicalRecord);
            await _context.SaveChangesAsync();
        }
    }
}