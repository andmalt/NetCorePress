using Microsoft.EntityFrameworkCore;
using NetCorePress.Authentication;
using NetCorePress.Models;

namespace NetCorePress.Services
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public CommentRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<bool> Save()
        {
            var saved = await _applicationDbContext.SaveChangesAsync();
            return saved >= 0;
        }

        public async Task<bool> ExistComment(int id)
        {
            return await _applicationDbContext.Comments
                .AnyAsync(c => c.Id == id);
        }

        public async Task<bool> Create(Comment comment)
        {
            await _applicationDbContext.AddAsync(comment);
            return await Save();
        }

        public async Task<Comment> GetComment(int id)
        {
            Comment? comment = await _applicationDbContext.Comments
                .SingleOrDefaultAsync(c => c.Id == id);

            return comment!;
        }

        public async Task<bool> Update(Comment comment)
        {
            _applicationDbContext.Update(comment);
            return await Save();
        }

        public async Task<bool> Delete(Comment comment)
        {
            _applicationDbContext.Remove(comment);
            return await Save();
        }
    }
}