using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using NetCorePress.Authentication;
using NetCorePress.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace NetCorePress.Services
{
    public class PostRepository : IPostRepository
    {
        ApplicationDbContext _applicationDbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostRepository(
            ApplicationDbContext applicationDbContext,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _applicationDbContext = applicationDbContext;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> Save()
        {
            var saved = await _applicationDbContext.SaveChangesAsync();
            return saved >= 0 ? true : false;
        }

        public async Task<bool> CreatePost(Post post)
        {
            // added a new post in the database
            await _applicationDbContext.AddAsync(post);
            return await Save();
        }

        public ICollection<Post> AllPost()
        {
            List<Post> posts = _applicationDbContext.Posts.ToList();
            return posts;
        }

        public async Task<Post> SelectPost(int id)
        {
            // Use SingleOrDefaultAsync to get the post with the specified id (if it exists)
            Post? post = await _applicationDbContext.Posts
                .SingleOrDefaultAsync(a => a.Id == id);

            return post!;
        }
    }
}