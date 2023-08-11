using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCorePress.Authentication;
using NetCorePress.Models;
using NetCorePress.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using NetCorePress.Dtos;
using NetCorePress.Models.Enums;

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
        [Route("all-posts")]
        public async Task<IActionResult> GetAllPosts()
        {

            var posts = await _postRepository.AllPost();

            if (posts.Count == 0)
            {
                return NotFound(string.Format("Non è stato trovato nessun articolo"));
            }
            var newPosts = new List<PostDto>();

            foreach (var post in posts)
            {
                var newPost = new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Message = post.Message,
                    UserId = post.UserId,
                    Category = post.Category,
                    CreationDate = post.CreationDate,
                    UpdateDate = post.UpdateDate
                };
                newPosts.Add(newPost);
            }

            var response = new Response<ICollection<PostDto>>
            {
                Success = true,
                Message = "Post elencati con successo!",
                Items = newPosts
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("get-post/{id}")]
        public async Task<IActionResult> GetPost(int id)
        {
            var existPost = await _postRepository.ExistPost(id);

            if (!existPost)
            {
                var resp = new Response
                {
                    Success = false,
                    Message = string.Format("Non è stato trovato il post!")
                };
                return NotFound(resp);
            }

            var post = await _postRepository.SelectPost(id);

            var newPost = new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Message = post.Message,
                UserId = post.UserId,
                Category = post.Category,
                CreationDate = post.CreationDate,
                UpdateDate = post.UpdateDate
            };

            var response = new Response<PostDto>
            {
                Success = true,
                Message = "Post trovato con successo!",
                Items = newPost
            };

            return Ok(response);
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
            // Set the creation date to now
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

            var newPost = new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Message = post.Message,
                UserId = post.UserId,
                Category = post.Category,
                CreationDate = post.CreationDate,
                UpdateDate = post.UpdateDate
            };

            var response = new Response<PostDto>
            {
                Success = true,
                Message = "Post creato con successo!",
                Items = newPost
            };

            return Ok(response);
        }

        [HttpPatch]
        [Route("update/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] PostPatchDTO postPatch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existPost = await _postRepository.ExistPost(id);

            if (!existPost)
            {
                var resp = new Response
                {
                    Success = false,
                    Message = string.Format("Non è stato trovato il post!")
                };
                return NotFound(resp);
            }

            var existingPost = await _postRepository.SelectPost(id);

            existingPost.Title = postPatch.Title;
            existingPost.Message = postPatch.Message;

            if (Enum.IsDefined(typeof(Category), postPatch.Category))
            {
                existingPost.Category = postPatch.Category;
            }
            else
            {
                ModelState.AddModelError("Category", "La categoria specificata non è valida.");
                return BadRequest(ModelState);
            }

            existingPost.UpdateDate = DateTime.Now;

            bool isUpdated = await _postRepository.UpdatePost(existingPost);

            if (!isUpdated)
            {
                ModelState.AddModelError("", $"Ci sono stati problemi nella modifica del post '{existingPost.Title}' ");
                return StatusCode(500, ModelState);
            }

            var response = new Response
            {
                Message = "Post modificato correttamente!",
                Success = true
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemovePost(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingPost = await _postRepository.SelectPost(id);

            if (existingPost == null)
            {
                var resp = new Response
                {
                    Success = false,
                    Message = string.Format("Non è stato trovato il post!")
                };
                return NotFound(resp);
            }

            bool isDeleted = await _postRepository.DeletePost(existingPost);

            if (!isDeleted)
            {
                ModelState.AddModelError("", $"Ci sono stati problemi nella cancellazione del post '{existingPost.Title}' ");
                return StatusCode(500, ModelState);
            }

            var response = new Response
            {
                Message = "Post cancellato correttamente!",
                Success = true
            };

            return Ok(response);
        }
    }
}