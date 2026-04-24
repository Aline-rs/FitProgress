using FitProgress.Domain.Entities;

namespace FitProgress.Api.Services.Auth
{
    public interface ITokenService
    {
        string GenerateToken(User user, DateTime expiresAt);
    }
}
