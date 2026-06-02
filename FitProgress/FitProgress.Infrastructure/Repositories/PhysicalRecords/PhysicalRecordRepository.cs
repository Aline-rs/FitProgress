
using Microsoft.EntityFrameworkCore;
using FitProgress.Application.PhysicalRecords.Interfaces;
using FitProgress.Domain.Entities;
using FitProgress.Infrastructure.Data;
using System.Reflection.Metadata.Ecma335;

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

        public async Task<IEnumerable<PhysicalRecord>> GetByUserIdAsync(Guid userId) => await _context.PhysicalRecords
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RecordDate)
                .ToListAsync();
    }
}