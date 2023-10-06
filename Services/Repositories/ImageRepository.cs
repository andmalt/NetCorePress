using Microsoft.AspNetCore.Identity;
using NetCorePress.Authentication;
using NetCorePress.Services.Repositories.Interfaces;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Gif;
using System.Security.Claims;

namespace NetCorePress.Services.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ImageRepository(
            IWebHostEnvironment webHostEnvironment,
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> SaveImage(IFormFile file)
        {
            var rootPath = _webHostEnvironment.ContentRootPath;

            var avatarDirectory = Path.Combine(rootPath, "Storage", "Avatar");

            if (!Directory.Exists(avatarDirectory))
            {
                Directory.CreateDirectory(avatarDirectory);
            }

            var filePath = Path.Combine(avatarDirectory, file.FileName);

            using var stream = new FileStream(filePath, FileMode.Create);

            try
            {
                await file.CopyToAsync(stream);
                // ClaimsPrincipal userClaimsPrincipal = _httpContextAccessor.HttpContext!.User;
                // var user = await _userManager.GetUserAsync(userClaimsPrincipal);
                // user.AvatarPath = filePath;
                // await _userManager.UpdateAsync(user);

                return true;
            }
            catch
            {
                return false;
            }


        }

        public async Task<bool> IsImageValid(IFormFile file)
        {
            try
            {
                using var stream = file.OpenReadStream();
                IImageFormat imageFormat = await Image.DetectFormatAsync(stream);

                if (imageFormat != null)
                {
                    var allowedFormats = new List<IImageFormat>
                        {
                            JpegFormat.Instance,
                            PngFormat.Instance,
                            GifFormat.Instance
                        };

                    return allowedFormats.Contains(imageFormat);
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsImageSizeValid(IFormFile file)
        {
            using var image = await Image.LoadAsync(file.OpenReadStream());
            var maxWidth = 1920; // maximum width allowed
            var maxHeight = 1080; // maximum height allowed

            return image.Width <= maxWidth && image.Height <= maxHeight;
        }
    }
}