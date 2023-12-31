using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCorePress.Authentication;
using NetCorePress.Services;
using NetCorePress.Services.Repositories.Interfaces;

namespace NetCorePress.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UserController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public UserController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpPost("upload/avatar")]
        public async Task<IActionResult> UploadAvatarImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new Response { Success = false, Message = "Invalid file" });
            }

            if (!await _imageRepository.IsImageValid(file))
            {
                return BadRequest(new Response { Success = false, Message = "Invalid image format" });
            }

            if (!await _imageRepository.IsImageSizeValid(file))
            {
                return BadRequest(new Response { Success = false, Message = "Image dimensions are too large" });
            }

            bool isSaved = await _imageRepository.SaveImage(file);

            if (!isSaved)
            {
                return BadRequest(new Response { Success = false, Message = "Image not saved" });
            }

            var response = new Response
            {
                Message = "Image uploaded successfully",
                Success = true,
            };

            return Ok(response);
        }
    }
}