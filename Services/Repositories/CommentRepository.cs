using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetCorePress.Authentication;
using NetCorePress.Dtos;
using NetCorePress.Models;
using NetCorePress.Services.Repositories.Interfaces;

namespace NetCorePress.Services.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public CommentRepository(
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

        public async Task<bool> ExistComment(int id)
        {
            return await _applicationDbContext.Comments
                .AnyAsync(c => c.Id == id);
        }

        public async Task<bool> Create(Comment comment)
        {
            comment.CreationDate = DateTime.Now;
            comment.UpdateDate = DateTime.Now;
            ClaimsPrincipal userClaimsPrincipal = _httpContextAccessor.HttpContext!.User;
            comment.UserId = _userManager.GetUserId(userClaimsPrincipal);

            await _applicationDbContext.AddAsync(comment);
            return await Save();
        }

        public async Task<Comment> GetComment(int id)
        {
            Comment? comment = await _applicationDbContext.Comments
                .SingleOrDefaultAsync(c => c.Id == id);

            return comment!;
        }

        public async Task<bool> Update(Comment comment, PatchComment patchComment)
        {
            comment.UpdateDate = DateTime.Now;
            comment.Text = patchComment.Text;

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