using System.ComponentModel.DataAnnotations;

namespace Wepapi_Management.Model
{
    public class ApplicationUser
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]

        public string PasswordHash { get; set; }
            
        public string Role { get; set; }
        public string ProfilePicture { get; set; }
    }
}
