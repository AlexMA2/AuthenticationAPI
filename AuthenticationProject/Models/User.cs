using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationProject.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Uid { get; set; }
        public string Username { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        [Required]
        public string Email { get; set; } = string.Empty;
        [Required]
        public UserMetadata Metadata { get; set; } = null!;
    }
    
    public class UserRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    public class UserLogin
    {
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
