using System.ComponentModel.DataAnnotations;

namespace FitProgress.Application.DTOs.Auth
{
    public class UserDTO
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
