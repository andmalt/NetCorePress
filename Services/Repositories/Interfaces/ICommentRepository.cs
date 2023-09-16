using NetCorePress.Dtos;
using NetCorePress.Models;

namespace NetCorePress.Services.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        Task<bool> Save();
        Task<bool> ExistComment(int id);
        Task<Comment> GetComment(int id);
        Task<bool> Create(Comment comment);
        Task<bool> Update(Comment comment, PatchComment patchComment);
        Task<bool> Delete(Comment comment);
    }
}