using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using NetCorePress.Models;

namespace NetCorePress.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(80)]
        public string? AvatarPath { get; set; }
        public virtual ICollection<Post>? Posts { get; set; }
        public virtual ICollection<Comment>? Comments { get; set; }
    }
}