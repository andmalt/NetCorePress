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
        /// <returns>A Boolean value, true if the image is saved or false otherwise.</returns>
        Task<bool> SaveImage(IFormFile file);
        /// <summary>
        ///     Checks whether the mime format of the image is allowed.
        /// </summary>
        /// <param name="file"></param>
        /// <returns>A Boolean value, true if the mime image is correct or false otherwise.</returns>
        Task<bool> IsImageValid(IFormFile file);
        /// <summary>
        ///     Check that the maximum size is not exceeded.
        /// </summary>
        /// <param name="file"></param>
        /// <returns>A Boolean value, true if the image size is correct, or false otherwise.</returns>
        Task<bool> IsImageSizeValid(IFormFile file);
    }
}