using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCorePress.Models;

namespace NetCorePress.Services
{
    public interface IPostRepository
    {
        Task<bool> Save();
        ICollection<Post> AllPost();
        Task<Post> SelectPost(int id);
        Task<bool> CreatePost(Post post);
    }
}