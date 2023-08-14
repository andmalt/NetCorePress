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
        Task<ICollection<Post>> AllPost();
        Task<PagedResult<Post>> GetPagedPosts(int page, int pageSize);
        Task<Post> SelectPost(int id);
        Task<bool> ExistPost(int id);
        Task<bool> CreatePost(Post post);
        Task<bool> UpdatePost(Post post);
        Task<bool> DeletePost(Post post);
    }
}