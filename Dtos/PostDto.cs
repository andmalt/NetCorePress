using System.ComponentModel.DataAnnotations;
using NetCorePress.Models;
using NetCorePress.Models.Enums;

namespace NetCorePress.Dtos
{
    public class PostDto
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Message { get; set; }

        public string? UserId { get; set; }

        public ICollection<CommentDto>? Comments { get; set; }

        public Category Category { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// PostDto is a data transfer object (DTO) of Post without comments initially,
        /// but if you want you can to add comments with function "AddComments".
        /// </summary>
        /// <param name="post"></param>
        public PostDto(Post post)
        {
            Id = post.Id;
            Title = post.Title;
            Message = post.Message;
            UserId = post.UserId;
            Category = post.Category;
            CreationDate = post.CreationDate;
            UpdateDate = post.UpdateDate;
        }

        public void AddComments(List<CommentDto> newComments)
        {
            Comments = newComments;
        }
    }

    public class PatchPostDTO
    {
        [Required]
        [MinLength(3, ErrorMessage = "Il titolo deve contenere almeno 3 caratteri")]
        [MaxLength(255, ErrorMessage = "Il titolo deve contenere al massimo 255 caratteri")]
        public string? Title { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Il messaggio deve contenere almeno un carattere")]
        public string? Message { get; set; }

        public Category Category { get; set; }
    }
}