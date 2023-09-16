using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetCorePress.Authentication;
using NetCorePress.Models;
using NetCorePress.Services;
using NetCorePress.Services.Repositories.Interfaces;
using NetCorePress.Dtos;
using NetCorePress.Models.Enums;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;

namespace NetCorePress.Controllers
{
    [EnableCors("myPolicy")]
    [ApiController]
    [Route("api/post")]
    public class PostController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPostRepository _postRepository;

        public PostController(
            IPostRepository postRepository,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager
            )
        {
            _postRepository = postRepository;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        [HttpGet]
        [Route("getpaged")]
        public async Task<IActionResult> GetPagedPosts(int page = 1, int pageSize = 10)
        {
            var pagedPosts = await _postRepository.GetPagedPosts(page, pageSize);

            if (pagedPosts.Data!.Count == 0)
            {
                return NotFound(string.Format("No posts were found"));
            }

            var newPosts = new List<PostDto>();

            foreach (var post in pagedPosts.Data)
            {
                var newComments = new List<CommentDto>();
                foreach (var comment in post.Comments!)
                {
                    var newComment = new CommentDto(comment);
                    newComments.Add(newComment);
                }

                var newPost = new PostDto(post);
                newPost.AddComments(newComments);

                newPosts.Add(newPost);
            }

            // * pagination
            var pagedPostsDto = new PagedResult<PostDto>
            {
                TotalItems = pagedPosts.TotalItems,
                Page = pagedPosts.Page,
                PageSize = pagedPosts.PageSize,
                Data = newPosts
            };

            return Ok(pagedPostsDto);
        }

        [HttpGet]
        [Route("all-posts")]
        public async Task<IActionResult> GetAllPosts()
        {

            var posts = await _postRepository.AllPost();


            if (posts.Count == 0)
            {
                return NotFound(string.Format("No posts were found"));
            }
            var newPosts = new List<PostDto>();

            foreach (var post in posts)
            {
                var newComments = new List<CommentDto>();
                foreach (var comment in post.Comments!)
                {
                    var newComment = new CommentDto(comment);
                    newComments.Add(newComment);
                }

                var newPost = new PostDto(post);
                newPost.AddComments(newComments);

                newPosts.Add(newPost);
            }

            var response = new Response<ICollection<PostDto>>
            {
                Success = true,
                Message = "Posts successfully listed!",
                Data = newPosts
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
                    Message = string.Format("The post was not found!")
                };
                return NotFound(resp);
            }

            var post = await _postRepository.SelectPost(id);

            var newListComments = new List<CommentDto>();

            foreach (var comment in post.Comments!)
            {
                var newComment = new CommentDto(comment);

                newListComments.Add(newComment);
            }

            var newPost = new PostDto(post);
            newPost.AddComments(newListComments);

            var response = new Response<PostDto>
            {
                Success = true,
                Message = "Post successfully found!",
                Data = newPost
            };

            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> CreatePost([FromBody] Post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isCreated = await _postRepository.CreatePost(post);

            if (!isCreated)
            {
                ModelState.AddModelError("", $"There were problems inserting the post '{post.Title}' ");
                return StatusCode(500, ModelState);
            }

            var response = new Response
            {
                Success = true,
                Message = "Post created successfully!",
            };

            return Ok(response);
        }

        [HttpPatch]
        [Route("update/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] PatchPostDTO patchPost)
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
                    Message = string.Format("The post was not found!")
                };
                return NotFound(resp);
            }

            var existingPost = await _postRepository.SelectPost(id);

            if (!Enum.IsDefined(typeof(Category), patchPost.Category))
            {
                ModelState.AddModelError("Category", "The specified category is invalid.");
                return BadRequest(ModelState);
            }

            bool isUpdated = await _postRepository.UpdatePost(existingPost, patchPost);

            if (!isUpdated)
            {
                ModelState.AddModelError("", $"There were problems editing the post '{existingPost.Title}' ");
                return StatusCode(500, ModelState);
            }

            var response = new Response
            {
                Message = "Post successfully edited!",
                Success = true
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> RemovePost(int id)
        {
            var existingPost = await _postRepository.SelectPost(id);

            if (existingPost == null)
            {
                var resp = new Response
                {
                    Success = false,
                    Message = string.Format("The post was not found!")
                };
                return NotFound(resp);
            }

            bool isDeleted = await _postRepository.DeletePost(existingPost);

            if (!isDeleted)
            {
                ModelState.AddModelError("delPost", $"There were problems deleting the post '{existingPost.Title}' ");
                return StatusCode(500, ModelState);
            }

            var response = new Response
            {
                Message = "Post successfully deleted!",
                Success = true
            };

            return Ok(response);
        }
    }
}