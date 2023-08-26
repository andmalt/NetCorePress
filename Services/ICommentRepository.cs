using NetCorePress.Models;

namespace NetCorePress.Services
{
    public interface ICommentRepository
    {
        Task<bool> Save();
        Task<bool> ExistComment(int id);
        Task<Comment> GetComment(int id);
        Task<bool> Create(Comment comment);
        Task<bool> Update(Comment comment);
        Task<bool> Delete(Comment comment);
    }
}