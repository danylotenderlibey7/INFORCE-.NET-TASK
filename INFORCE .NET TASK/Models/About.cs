using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INFORCE_.NET_TASK.Models
{
    public class About
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(500)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        [Display(Name = "Algorithm Description")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Last Updated")]
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        public Guid? UpdatedByUserId { get; set; }

        [ForeignKey ("UpdatedByUserId")]
        public virtual User? UpdatedBy { get; set; }
    }
}
