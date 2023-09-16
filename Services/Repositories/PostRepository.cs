using NetCorePress.Authentication;
using NetCorePress.Models;
using NetCorePress.Services.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using NetCorePress.Dtos;
using NetCorePress.Models.Enums;


namespace NetCorePress.Services.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _applicationDbContext;

        public PostRepository(
            ApplicationDbContext applicationDbContext,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager
        )
        {
            _applicationDbContext = applicationDbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<bool> Save()
        {
            var saved = await _applicationDbContext.SaveChangesAsync();
            return saved >= 0;
        }

        public async Task<bool> ExistPost(int id)
        {
            return await _applicationDbContext
            .Posts
            .AnyAsync(c => c.Id == id);
        }

        // ! query without pagination
        public async Task<ICollection<Post>> AllPost()
        {
            List<Post> posts = await _applicationDbContext.Posts
                .Include(p => p.Comments)
                .ToListAsync();

            return posts;
        }

        // ! query with pagination
        public async Task<PagedResult<Post>> GetPagedPosts(int page, int pageSize)
        {
            var totalItems = await _applicationDbContext.Posts.CountAsync();
            var posts = await _applicationDbContext.Posts
                .Include(p => p.Comments)
                .OrderByDescending(p => p.UpdateDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Post>
            {
                // 
                TotalItems = totalItems,
                // page number
                Page = page,
                // items per page
                PageSize = pageSize,
                //
                Data = posts
            };
        }


        public async Task<Post> SelectPost(int id)
        {
            // Use SingleOrDefaultAsync to get the post with the specified id (if it exists)
            Post? post = await _applicationDbContext.Posts
                .Include(p => p.Comments)
                .SingleOrDefaultAsync(p => p.Id == id);

            return post!;
        }

        public async Task<bool> CreatePost(Post post)
        {
            post.CreationDate = DateTime.Now;
            post.UpdateDate = DateTime.Now;
            ClaimsPrincipal userClaimsPrincipal = _httpContextAccessor.HttpContext!.User;
            post.UserId = _userManager.GetUserId(userClaimsPrincipal);

            // added a new post in the database
            await _applicationDbContext.AddAsync(post);
            return await Save();
        }

        /// <summary>
        /// Updates the post.
        /// </summary>
        /// <param name="post">Post to edit</param>
        /// <param name="patchPost">Post edited</param>
        /// <returns>bool</returns> <summary>
        public async Task<bool> UpdatePost(Post post, PatchPostDTO patchPost)
        {
            post.Title = patchPost.Title;
            post.Message = patchPost.Message;
            post.UpdateDate = DateTime.Now;

            if (Enum.IsDefined(typeof(Category), patchPost.Category))
            {
                post.Category = patchPost.Category;
            }

            _applicationDbContext.Update(post);
            return await Save();
        }

        public async Task<bool> DeletePost(Post post)
        {
            _applicationDbContext.Remove(post);
            return await Save();
        }

    }
}