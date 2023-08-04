namespace Portfolio.API.Services.PhotoService
{
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public interface IPhotoService
    {
        /// <summary>
        /// With this method the user will can upload his own photos.
        /// </summary>
        /// <param name="formFile"></param>
        /// <returns></returns>
        Task<ImageUploadResult> UploadPhotoAsync(IFormFile formFile);

        /// <summary>
        /// The method should return the own profile user's image
        /// </summary>
        /// <returns></returns>
        Task<string> GetUserProfileImageUrl(string userId);
    }
}
