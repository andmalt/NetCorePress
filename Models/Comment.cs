using System.ComponentModel.DataAnnotations;
using NetCorePress.Authentication;

namespace NetCorePress.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Text { get; set; }

        [Required]
        public int PostId { get; set; }

        public string? UserId { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public virtual ApplicationUser? User { get; set; }

        public virtual Post? Post { get; set; }
    }
}