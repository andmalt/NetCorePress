using System.ComponentModel.DataAnnotations;
using NetCorePress.Models.Enums;
using NetCorePress.Authentication;

namespace NetCorePress.Models
{
    public class Post
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [MinLength(3, ErrorMessage = "Il titolo deve contenere almeno 3 caratteri")]
        [MaxLength(255, ErrorMessage = "Il titolo deve contenere al massimo 255 caratteri")]
        public string? Title { get; set; }

        [MinLength(1, ErrorMessage = "Il messaggio deve contenere almeno un carattere")]
        public string? Message { get; set; }

        [Required]
        public string? UserId { get; set; }

        [Required]
        public Category Category { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        // below the relationship with users
        public virtual ApplicationUser? User { get; set; }
    }
}