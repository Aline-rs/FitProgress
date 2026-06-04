using FitProgress.Application.DTOs.Auth;
using FitProgress.Application.Services.Auth;
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
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            var response = await _authService.LoginAsync(request);

            if (response is null)
            {
                return Unauthorized(new
                {
                    message = "E-mail ou senha inválidos."
                });
            }

            return Ok(response);
        }

        [HttpPost("register")] 
        public async Task<IActionResult> Register([FromBody] UserDTO request)
        {
            var result = await _authService.RegisterAsync(request);

            if (!result.Success)

                return BadRequest(new { message = result.Message });
                return (Created(string.Empty, new { id = result.Data }));         
        }        
    }
}
