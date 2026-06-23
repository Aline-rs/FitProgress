using FitProgress.Application.Photos.Interfaces;
using FitProgress.Domain.Entities;
using FitProgress.Infrastructure.Data;

namespace FitProgress.Infrastructure.Repositories.ProgressPhotos
{
    public class ProgressPhotoRepository : IProgressPhotoRepository
    {
        private readonly AppDbContext _context;

        public ProgressPhotoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ProgressPhoto progressPhoto)
        {
            await _context.ProgressPhotos.AddAsync(progressPhoto);
            await _context.SaveChangesAsync();
        }
    }
}