using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCorePress.Authentication;
using NetCorePress.Dtos;
using NetCorePress.Services;
using NetCorePress.Services.Repositories.Interfaces;
using NetCorePress.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Cors;
using NetCorePress.Services.Emails;

namespace NetCorePress.Controllers
{
    [EnableCors("myPolicy")]
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
        public async Task<IActionResult> GetComment(int id)
        {
            var exists = await _commentRepository.ExistComment(id);
            if (!exists)
            {
                var resp = new Response()
                {
                    Success = false,
                    Message = "No comments were found"
                };
                return NotFound(resp);
            }

            var comment = await _commentRepository.GetComment(id);

            var newComment = new CommentDto(comment);

            var response = new Response<CommentDto>
            {
                Success = true,
                Message = "Comment successfully found",
                Data = newComment
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreateComment([FromBody] Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isCreated = await _commentRepository.Create(comment);

            if (!isCreated)
            {
                ModelState.AddModelError("", $"There were problems inserting the comment '{comment.Text}' ");
                return StatusCode(500, ModelState);
            }

            var emailService = new EmailService();

            var comm = await _commentRepository.GetComment(comment.Id);

            var toEmail = await _userManager.GetEmailAsync(comm.Post!.User!);

            string emailTemplate = System.IO.File.ReadAllText("./Services/Emails/Views/email.comment.html");
            emailTemplate = emailTemplate.Replace("{{comment.Email}}", comm.User!.Email);
            emailTemplate = emailTemplate.Replace("{{comment.Text}}", comm.Text);

            bool emailSent = await emailService.SendEmailAsync("info.site@example.com", toEmail, "You received a comment", emailTemplate, true);

            if (emailSent)
            {
                var response = new Response
                {
                    Success = true,
                    Message = "The comment is created and the email successfully sent"
                };
                return Ok(response);
            }
            else
            {
                var response = new Response
                {
                    Success = false,
                    Message = "Email not sent"
                };
                return Ok(response);
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] PatchComment patchComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Comment comment = await _commentRepository.GetComment(id);

            if (comment is null)
            {
                var res = new Response
                {
                    Success = false,
                    Message = "Comment not found!",
                };
                return NotFound(res);
            }

            var isUpdated = await _commentRepository.Update(comment, patchComment);

            if (isUpdated is false)
            {
                var res = new Response
                {
                    Success = false,
                    Message = $"You were unable to edit the comment with ID:{comment.Id}",
                };
                return BadRequest(res);
            }

            var response = new Response
            {
                Success = true,
                Message = "You were able to edit the comment"
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("delete")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            Comment comment = await _commentRepository.GetComment(id);

            if (comment is null)
            {
                var res = new Response
                {
                    Success = false,
                    Message = $"Comment with id{id} not found!"
                };
                return NotFound(res);
            }

            bool isDeleted = await _commentRepository.Delete(comment);

            if (!isDeleted)
            {
                var res = new Response
                {
                    Success = false,
                    Message = $"There is a problem to delete comment with id{comment.Id}"
                };
                return BadRequest(res);
            }

            var response = new Response
            {
                Success = true,
                Message = "Comment successfully deleted"
            };
            return Ok(response);
        }
    }
}