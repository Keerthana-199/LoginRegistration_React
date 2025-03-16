using System.ComponentModel.DataAnnotations;

namespace LoginRegisterAuthAPI.Models
{
    public class Users
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty; // Store hashed password

        public string? Phone { get; set; }

        public int isActive { get; set; } = 1;
    }
}

