using FitProgress.Application.Common;
using FitProgress.Application.DTOs.Auth;
using FitProgress.Application.Settings;
using FitProgress.Application.Users.Interfaces;
using FitProgress.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FitProgress.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            ITokenService tokenService,
            IUserRepository userRepository,
            IOptions<JwtSettings> jwtSettings)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<ServiceResult<LoginResponseDTO>> LoginAsync(LoginRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return ServiceResult<LoginResponseDTO>.Fail(
                    "E-mail e senha são obrigatórios.");
            }

            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user is null)
            {
                return ServiceResult<LoginResponseDTO>.Fail(
                    "E-mail ou senha inválidos.");
            }

            var passwordHasher = new PasswordHasher<User>();

            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password
            );

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return ServiceResult<LoginResponseDTO>.Fail(
                    "E-mail ou senha inválidos.");
            }

            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

            var token = _tokenService.GenerateToken(user, expiresAt);

            var response = new LoginResponseDTO
            {
                Token = token,
                ExpiresAt = expiresAt,
                User = new UserLoginResponse
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email
                }
            };

            return ServiceResult<LoginResponseDTO>.Ok(response);
        }
    }
}
