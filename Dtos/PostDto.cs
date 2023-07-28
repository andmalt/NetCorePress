using NetCorePress.Models.Enums;

namespace NetCorePress.Dtos
{
    public class PostDto
    {
        public int Id { get; set; }

        public string? Title { get; set; }

        public string? Message { get; set; }

        public string? UserId { get; set; }

        public Category Category { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}