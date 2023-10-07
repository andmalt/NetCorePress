using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCorePress.Dtos;
using NetCorePress.Models;

namespace NetCorePress.Services.Repositories.Interfaces
{
    public interface IPostRepository
    {
        /// <summary>
        /// Save the changes to your database.
        /// </summary>
        /// <returns>A Boolean value, true if the save was successful or false otherwise.</returns>
        Task<bool> Save();
        /// <summary>
        /// Get all post in your database.
        /// </summary>
        /// <returns>A list of post</returns>
        Task<ICollection<Post>> AllPost();
        /// <summary>
        /// Get all post in your database.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns>A list of paginated posts</returns>
        Task<PagedResult<Post>> GetPagedPosts(int page, int pageSize);
        /// <summary>
        /// Get the post in the database you indicated with the id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A post with you selected.</returns>
        Task<Post> SelectPost(int id);
        /// <summary>
        /// Check if the post exist in the database. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Boolean value, true if the post exists or false otherwise.</returns>
        Task<bool> ExistPost(int id);
        /// <summary>
        /// Create a post.
        /// </summary>
        /// <param name="post"></param>
        /// <returns>A Boolean value, true if the post has been created or false otherwise.</returns>
        Task<bool> CreatePost(Post post);
        /// <summary>
        /// Update the post.
        /// </summary>
        /// <param name="post">Post to edit</param>
        /// <param name="patchPost">Post edited</param>
        /// <returns>A Boolean value, true if the post has been updated or false otherwise.</returns>
        Task<bool> UpdatePost(Post post, PatchPostDTO postPatch);
        /// <summary>
        /// Delete the post
        /// </summary>
        /// <param name="post"></param>
        /// <returns>A Boolean value, true if the post has been deleted or false otherwise.</returns>
        Task<bool> DeletePost(Post post);
    }
}