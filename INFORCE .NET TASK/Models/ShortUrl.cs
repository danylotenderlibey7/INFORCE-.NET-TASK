using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INFORCE_.NET_TASK.Models
{
    public class ShortUrl
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "URL is required")]
        [Url(ErrorMessage = "Enter a valid URL")]
        [Display(Name = "Original URL")]
        [StringLength(1500, ErrorMessage = "URL is too long")]
        public string OriginalUrl { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Short Code")]
        [StringLength(10)]
        public string ShortCode { get; set; } = string.Empty;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public Guid CreatedByUserId { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedBy { get; set; } = null!;

        public ShortUrl() 
        {
        
        }
    }
}
