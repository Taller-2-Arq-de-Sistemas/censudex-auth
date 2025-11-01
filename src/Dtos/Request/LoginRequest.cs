using System.ComponentModel.DataAnnotations; 

namespace censudex_auth_service.src.Dtos.Request
{
    public class LoginRequest
    {
        [Required]
        public string Identifier { get; set; } = null!; // username or email

        [Required]
        public string Password { get; set; } = null!; 
    }
}