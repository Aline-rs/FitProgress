using FitProgress.Api.DTOs.Auth;
using FitProgress.Api.Services.Auth;
using FitProgress.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FitProgress.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase 
    {

        private readonly IAuthService _authService;

        public AuthController(
            IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authService.LoginAsync(request);

            if(response is null)
            {
                return Unauthorized(new
                {
                    message = "E-mail ou senha inválidos."
                });
            }

            return Ok(response);
        }

        // TODO: remover endpoint temporário de geração de hash quando a API de registro estiver pronta
        [HttpGet("generate-password-hash")]
        public IActionResult GeneratePasswordHash()
        {
            var passwordHasher = new PasswordHasher<User>();

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = "Aline",
                Email = "aline.rosa@email.com"
            };

            var hash = passwordHasher.HashPassword(user, "123456");

            return Ok(hash);
        }
    }
}
