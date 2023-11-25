namespace Portfolio.API.Services.Contracts
{
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;

    public interface ICloudinaryService
    {
        /// <summary>
        /// Upload some image to cloudinary service.
        /// </summary>
        /// <param name="file"></param>
        /// <param name="heigth"></param>
        /// <param name="width"></param>
        /// <param name="publicId"></param>
        /// <returns></returns>
        Task<ImageUploadResult> UploadImageToCloudinaryAsync(IFormFile file, int heigth, int width, string publicId);

        /// <summary>
        /// Delete all project images from cloudinary service.
        /// </summary>
        /// <param name="publicId"></param>
        /// <returns></returns>
        Task DeleteImagesAsync(List<string> projectPublicIds);
    }
}
