namespace Portfolio.API.Services.PhotoService
{
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Portfolio.API.Data.Models;
    using Portfolio.Data.Repositories;

    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary cloudinary;
        private readonly IRepository<UserImage> userImageRepository;
        public CloudinaryService(
            IConfiguration configuration,
            IRepository<UserImage> userImageRepository)
        {
            Account account = new Account(
                configuration["CloudinarySettings:Name"],
                configuration["CloudinarySettings:ApiKey"],
                configuration["CloudinarySettings:SecretKey"]);

                cloudinary = new Cloudinary(account);
            this.userImageRepository = userImageRepository;
        }
        public async Task<string> GetUserProfilePictureUrlAsync(string userId)
        {
            var user = await this.userImageRepository.AllAsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId);

            if (user == null || string.IsNullOrEmpty(user.ProfileImageUrl))
            {
                throw new Exception("User or profile image not found.");
            }

            return user.ProfileImageUrl;
        }

        public async Task<ImageUploadResult> UploadProfilePictureAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation()
                    .Height(600).Width(600)
                };
                uploadResult = await cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }
    }
}
