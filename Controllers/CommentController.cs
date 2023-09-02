using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCorePress.Authentication;
using NetCorePress.Dtos;
using NetCorePress.Services;
using NetCorePress.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Cors;

namespace NetCorePress.Controllers
{
    [ApiController]
    [Route("api/comment")]
    public class CommentController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICommentRepository _commentRepository;

        public CommentController(
                UserManager<ApplicationUser> userManager,
                IHttpContextAccessor httpContextAccessor,
                ICommentRepository commentRepository
            )
        {
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _commentRepository = commentRepository;
        }

        [HttpGet]
        [Route("getcomment/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var exists = await _commentRepository.ExistComment(id);
            if (!exists)
            {
                var resp = new Response()
                {
                    Success = false,
                    Message = "Non è stato trovato nessun commento"
                };
                return NotFound(resp);
            }

            var comment = await _commentRepository.GetComment(id);

            var newComment = new CommentDto()
            {
                Id = comment.Id,
                Text = comment.Text,
                PostId = comment.PostId,
                UserId = comment.UserId,
                CreationDate = comment.CreationDate,
                UpdateDate = comment.UpdateDate,
            };

            var response = new Response<CommentDto>
            {
                Success = true,
                Message = "Commento trovato con successo",
                Data = newComment
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Create([FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Set the creation date to now
            comment.CreationDate = DateTime.Now;
            comment.UpdateDate = DateTime.Now;
            ClaimsPrincipal userClaimsPrincipal = _httpContextAccessor.HttpContext!.User;
            comment.UserId = _userManager.GetUserId(userClaimsPrincipal);

            var isCreated = await _commentRepository.Create(comment);

            if (!isCreated)
            {
                ModelState.AddModelError("", $"Ci sono stati problemi nell'inserimento del post '{comment.Text}' ");
                return StatusCode(500, ModelState);
            }

            var newComment = new CommentDto()
            {
                Id = comment.Id,
                Text = comment.Text,
                PostId = comment.PostId,
                UserId = comment.UserId,
                CreationDate = comment.CreationDate,
                UpdateDate = comment.UpdateDate,
            };

            var response = new Response<CommentDto>()
            {
                Success = true,
                Message = "The comment is created succesfully",
                Data = newComment
            };

            return Ok(response);
        }
    }
}