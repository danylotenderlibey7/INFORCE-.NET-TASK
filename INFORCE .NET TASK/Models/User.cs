using System.ComponentModel.DataAnnotations;

namespace INFORCE_.NET_TASK.Models
{
    public enum UserRole
    {
        User = 0,
        Admin = 1
    }

    public class User 
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Login is required")]
        [StringLength(50, ErrorMessage = "Login must be 5-50 characters", MinimumLength = 5)]
        [Display(Name = "Login")]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } = UserRole.User;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<ShortUrl> ShortUrls { get; set; } = new HashSet<ShortUrl>();

        public User()
        {

        }
    }
}
