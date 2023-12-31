using System.ComponentModel.DataAnnotations;
using NetCorePress.Models.Enums;
using NetCorePress.Authentication;

namespace NetCorePress.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Il titolo deve contenere almeno 3 caratteri")]
        [MaxLength(255, ErrorMessage = "Il titolo deve contenere al massimo 255 caratteri")]
        public string? Title { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Il messaggio deve contenere almeno un carattere")]
        public string? Message { get; set; }

        public string? UserId { get; set; }

        [Required]
        public Category Category { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        // below the relationship with users and comments
        public virtual ApplicationUser? User { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }
    }
}