using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCorePress.Services.Repositories.Interfaces
{
    public interface IImageRepository
    {
        /// <summary>
        ///     Save the image to the Storage/Avatar directory.
        /// </summary>
        /// <param name="file"></param>
        /// <returns>void</returns>
        Task<bool> SaveImage(IFormFile file);
        /// <summary>
        ///     Checks whether the mime format of the image is allowed.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<bool> IsImageValid(IFormFile file);
        /// <summary>
        ///     Check the maximum size
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<bool> IsImageSizeValid(IFormFile file);
    }
}