namespace Portfolio.API.Services.PhotoService
{
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

    public interface ICloudinaryService
    {
        /// <summary>
        /// Every time this method is invoked, the user will be able to upload their own profile picture.
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        Task<ImageUploadResult> UploadProfilePictureAsync(IFormFile formFile);

        /// <summary>
        /// The method should retrieve the user's profile picture from the database and return it.
        /// </summary>
        /// <returns></returns>
        Task<string> GetUserProfilePictureUrlAsync(string userId);
    }
}
