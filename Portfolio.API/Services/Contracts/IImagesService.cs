namespace Portfolio.API.Services.Contracts
{
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.ImagesDtos;

    public interface IImagesService
    {
        /// <summary>
        /// Upload some image to cloudinary service.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="heigth"></param>
        /// <param name="width"></param>
        /// <param name="publicId"></param>
        /// <returns></returns>
        Task<ImageUploadResult> UploadImageToCloudinary(IFormFile file, int heigth, int width, string publicId);

        Task<string> GetProjectDetailsImageUrlAsync(int projectId);

        /// <summary>
        /// The method should retrieve the user's profile image from the database and return it.
        /// </summary>
        /// <returns></returns>
        Task<string> GetUserProfileImageUrlAsync(string userId);

        /// <summary>
        /// The method should retrieve the user's home page image from the database and return it.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GetUserHomePageImageUrlAsync(string userId);


        /// <summary>
        /// The method should retrieve the user's about image from the database and return it.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<string> GetAboutImageUrlAsync(string userId);


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
        Task<UploadImageDto> SaveProjectImageToDatabase(string imageUrl, Project image, int projectId);

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
