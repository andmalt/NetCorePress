using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCorePress.Authentication;
using NetCorePress.Models;
using NetCorePress.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using NetCorePress.Dtos;

namespace NetCorePress.Controllers
{
    [ApiController]
    [Route("api/post")]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PostController(
            IPostRepository postRepository,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _postRepository = postRepository;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        [Route("create")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Create([FromBody] Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Imposta la data di creazione al momento attuale
            post.CreationDate = DateTime.Now;
            post.UpdateDate = DateTime.Now;
            ClaimsPrincipal userClaimsPrincipal = _httpContextAccessor.HttpContext!.User;
            post.UserId = _userManager.GetUserId(userClaimsPrincipal);

            var isCreated = await _postRepository.CreatePost(post);

            if (!isCreated)
            {
                ModelState.AddModelError("", $"Ci sono stati problemi nell'inserimento del post '{post.Title}' ");
                return StatusCode(500, ModelState);
            }

            var newPost = new PostDto();
            newPost.Id = post.Id;
            newPost.Title = post.Title;
            newPost.Message = post.Message;
            newPost.UserId = post.UserId;
            newPost.Category = post.Category;
            newPost.CreationDate = post.CreationDate;
            newPost.UpdateDate = post.UpdateDate;

            var response = new Response<PostDto>();
            response.Success = true;
            response.Message = "Post creato con successo!";
            response.Items = newPost;

            return Ok(response);
        }
    }
}