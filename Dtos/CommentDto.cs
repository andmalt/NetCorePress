using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCorePress.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string? Text { get; set; }

        public int PostId { get; set; }

        public string? UserId { get; set; }

        public DateTime? CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }
    }
}