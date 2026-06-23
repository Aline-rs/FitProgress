using FitProgress.Domain.Entities;

namespace FitProgress.Application.Photos.Interfaces
{
    public interface IProgressPhotoRepository
    {
        Task AddAsync(ProgressPhoto progressPhoto);
    }
}