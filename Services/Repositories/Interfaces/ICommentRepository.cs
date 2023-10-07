using NetCorePress.Dtos;
using NetCorePress.Models;

namespace NetCorePress.Services.Repositories.Interfaces
{
    public interface ICommentRepository
    {
        /// <summary>
        /// Save the changes to your database.
        /// </summary>
        /// <returns>A Boolean value, true if the save was successful or false otherwise.</returns>
        Task<bool> Save();
        /// <summary>
        /// Check if the comment exist in the database. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Boolean value, true if the comment exists or false otherwise.</returns>
        Task<bool> ExistComment(int id);
        /// <summary>
        /// Get the comment in the database you indicated with the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A comment with you selected.</returns>
        Task<Comment> GetComment(int id);
        /// <summary>
        /// Create a comment.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>A Boolean value, true if the comment has been created or false otherwise.</returns>
        Task<bool> Create(Comment comment);
        /// <summary>
        /// Update the comment.
        /// </summary>
        /// <param name="comment"></param>
        /// <param name="patchComment"></param>
        /// <returns>A Boolean value, true if the comment has been updated or false otherwise.</returns>
        Task<bool> Update(Comment comment, PatchComment patchComment);
        /// <summary>
        /// Delete the comment.
        /// </summary>
        /// <param name="comment"></param>
        /// <returns>A Boolean value, true if the comment has been deleted or false otherwise.</returns>
        Task<bool> Delete(Comment comment);
    }
}