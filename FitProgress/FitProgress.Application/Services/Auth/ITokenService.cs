using FitProgress.Domain.Entities;

namespace FitProgress.Application.Services.Auth
{
    public interface ITokenService
    {
        string GenerateToken(User user, DateTime expiresAt);
    }
}
