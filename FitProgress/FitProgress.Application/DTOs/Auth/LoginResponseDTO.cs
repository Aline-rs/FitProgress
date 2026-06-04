namespace FitProgress.Application.DTOs.Auth
{
    public class LoginResponseDTO
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public UserLoginResponse User { get; set; } = null!;
    }

    public class UserLoginResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}