using FitProgress.Application.Common;
using FitProgress.Application.DTOs.Auth;

namespace FitProgress.Application.Services.Auth
{
    public interface IAuthService
    {
        Task<ServiceResult<LoginResponseDTO>> LoginAsync(LoginRequestDTO request);
    }
}