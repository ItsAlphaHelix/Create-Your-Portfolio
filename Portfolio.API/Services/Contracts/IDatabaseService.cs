using Portfolio.API.Data.Models;
using Portfolio.API.Dtos.ImagesDtos;

namespace Portfolio.API.Services.Contracts
{
    public interface IDatabaseService
    {
        /// <summary>
        /// Save image URL to database.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="image"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UploadImageDto> SaveImageUrlToDatabaseAsync(string imageUrl, UserImage image, string userId);

        /// <summary>
        /// Saving project image to database.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="image"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<UploadImageDto> SaveProjectImageToDatabaseAsync(string imageUrl, Project image, int projectId);

        /// <summary>
        /// Retrieve the existing image URL by project ID and update it.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="image"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<UploadImageDto> EditProjectImageToDatabaseAsync(string imageUrl, Project image, int projectId);

        /// <summary>
        /// Retrieve the existing image URL by user ID and update it.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="image"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UploadImageDto> EditImageUrlInDatabaseAsync(string imageUrl, UserImage image, string userId);
    }
}
