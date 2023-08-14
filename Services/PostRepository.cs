using NetCorePress.Authentication;
using NetCorePress.Models;
using Microsoft.EntityFrameworkCore;


namespace NetCorePress.Services
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public PostRepository(
            ApplicationDbContext applicationDbContext
        )
        {
            _applicationDbContext = applicationDbContext;
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
            // added a new post in the database
            await _applicationDbContext.AddAsync(post);
            return await Save();
        }

        public async Task<bool> UpdatePost(Post post)
        {
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