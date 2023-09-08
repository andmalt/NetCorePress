
using NetCorePress.Models;

namespace NetCorePress.Dtos
{
    /// <summary>
    /// 
    /// </summary> <summary>
    /// 
    /// </summary>
    public class CommentDto
    {
        public int Id { get; set; }
        public string? Text { get; set; }

        public int PostId { get; set; }

        public string? UserId { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

        public CommentDto(Comment comment)
        {
            Id = comment.Id;
            Text = comment.Text;
            PostId = comment.PostId;
            UserId = comment.UserId;
            CreationDate = comment.CreationDate;
            UpdateDate = comment.UpdateDate;
        }
    }
}