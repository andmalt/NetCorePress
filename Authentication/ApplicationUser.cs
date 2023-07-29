using Microsoft.AspNetCore.Identity;
using NetCorePress.Models;

namespace NetCorePress.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Post>? Posts { get; set; }

        public virtual ICollection<Comment>? Comments { get; set; }
    }
}