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
        Task<ImageUploadResult> UploadProfileImageAsync(IFormFile formFile);

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
        Task<ImageUploadResult> UploadHomePageImageAsync(IFormFile file);

        /// <summary>
        /// Saving image url to the database.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="userProfileImage"></param>
        /// <param name="userHomePageImage"></param>
        /// <returns></returns>
        Task<UploadImageDto> SaveImageUrlToDatabase(string imageUrl, UserImage image);

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
        public Task<ImageUploadResult> UploadAboutImageAsync(IFormFile file);

        /// <summary>
        /// The method should retrieve the user's about image from the database and return it.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<string> GetAboutImageUrlAsync(string userId);
    }
}
