namespace Portfolio.API.Services.Contracts
{
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Http;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.ImagesDtos;

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
    }
}
