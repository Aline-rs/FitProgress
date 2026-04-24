using FitProgress.Api.DTOs.Auth;

namespace FitProgress.Api.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginResponse?> LoginAsync(LoginRequest request);
    }
}