namespace Portfolio.API.Services.Contracts
{
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.ImagesDtos;

    public interface IImagesService
    {
        /// <summary>
        /// Every time this method is invoked, the user will be able to upload their own profile picture to the Cloudinary service.
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        Task<ImageUploadResult> UploadProfileImageAsync(IFormFile formFile, string userId);

        /// <summary>
        /// The method should retrieve the user's profile image from the database and return it.
        /// </summary>
        /// <returns></returns>
        Task<string> GetUserProfileImageUrlAsync(string userId);

        /// <summary>
        /// Every time this method is invoked, the user will be able to upload their own home page picture to the Cloudinary service.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<ImageUploadResult> UploadHomePageImageAsync(IFormFile file, string userId);

        /// <summary>
        /// The method should retrieve the user's home page image from the database and return it.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GetUserHomePageImageUrlAsync(string userId);

        /// <summary>
        /// Uploading about user image to cloudinary service.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public Task<ImageUploadResult> UploadAboutImageAsync(IFormFile file, string userId);

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
        /// Retrieve the existing image URL by user ID and update it.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="image"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UploadImageDto> EditImageUrlInDatabaseAsync(string imageUrl, UserImage image, string userId);
    }
}
