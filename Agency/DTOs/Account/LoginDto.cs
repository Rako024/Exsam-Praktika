using System.ComponentModel.DataAnnotations;

namespace Agency.DTOs.Account
{
    public class LoginDto
    {
        [Required]
        [MaxLength(255)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
