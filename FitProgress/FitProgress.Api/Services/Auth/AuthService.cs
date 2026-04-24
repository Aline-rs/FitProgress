using FitProgress.Api.DTOs.Auth;
using FitProgress.Api.Settings;
using FitProgress.Domain.Entities;
using FitProgress.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FitProgress.Api.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly JwtSettings _jwtSettings;

        public AuthService(
            AppDbContext context,
            ITokenService tokenService,
            IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _tokenService = tokenService;
            _jwtSettings = jwtSettings.Value;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                return null;
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user is null)
            {
                return null;
            }

            var passwordHasher = new PasswordHasher<User>();

            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                request.Password
            );

            if (passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                return null;
            }

            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes);

            var token = _tokenService.GenerateToken(user, expiresAt);

            return new LoginResponse
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
        }
    }
}
