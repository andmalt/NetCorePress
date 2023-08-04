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

        public async Task<ICollection<Post>> AllPost()
        {
            List<Post> posts = await _applicationDbContext.Posts.ToListAsync();
            return posts;
        }

        public async Task<Post> SelectPost(int id)
        {
            // Use SingleOrDefaultAsync to get the post with the specified id (if it exists)
            Post? post = await _applicationDbContext.Posts
                .SingleOrDefaultAsync(a => a.Id == id);

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