namespace Portfolio.API.Services.PhotoService
{
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.ImagesDtos;

    public interface IImageService
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
        Task<UploadImageDto> SaveImageUrlToDatabase(
            string imageUrl, UserProfileImage userProfileImage = null, UserHomePageImage userHomePageImage = null);

        /// <summary>
        /// Getting from cloudinary user's home page image url.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GetUserHomePageImageUrlAsync(string userId);

        /// <summary>
        /// The method should retrieve the user's home page image from the database and return it.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GetUserHomepageImageUrlAsync(string userId);
    }
}
