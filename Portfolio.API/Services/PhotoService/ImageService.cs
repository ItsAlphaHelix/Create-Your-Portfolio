namespace Portfolio.API.Services.PhotoService
{
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.ImagesDtos;
    using Portfolio.Data.Repositories;

    public class ImageService: IImageService
    {
        private readonly Cloudinary cloudinary;
        private readonly IRepository<UserProfileImage> userProfileImageRepository;
        private readonly IRepository<UserHomePageImage> userHomePageRepository;
        public ImageService(
            IConfiguration configuration,
            IRepository<UserProfileImage> userProfileImageRepository,
            IRepository<UserHomePageImage> userHomePageRepository)
        {
            Account account = new Account(
                configuration["CloudinarySettings:Name"],
                configuration["CloudinarySettings:ApiKey"],
                configuration["CloudinarySettings:SecretKey"]);

            cloudinary = new Cloudinary(account);
            this.userProfileImageRepository = userProfileImageRepository;
            this.userHomePageRepository = userHomePageRepository;
        }

        public async Task<string> GetUserHomePageImageUrlAsync(string userId)
        {
            var user = await this.userHomePageRepository.AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => new UploadImageDto()
                {
                    ImageUrl = x.HomePageImageUrl

                }).FirstOrDefaultAsync();

            if (user == null || string.IsNullOrEmpty(user.ImageUrl))
            {
                throw new Exception("User or profile image not found.");
            }

            return user.ImageUrl;
        }

        public async Task<string> GetUserProfileImageUrlAsync(string userId)
        {
            var user = await this.userProfileImageRepository.AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => new UploadImageDto()
                {
                    ImageUrl = x.ProfileImageUrl
                })
                .FirstOrDefaultAsync();

            if (user == null || string.IsNullOrEmpty(user.ImageUrl))
            {
                throw new Exception("User or profile image not found.");
            }

            return user.ImageUrl;
        }

        public async Task<UploadImageDto> SaveUserProfileImageToDatabase(string imageUrl, UserProfileImage photo)
        {
            await this.userProfileImageRepository.AddAsync(photo);
            await this.userProfileImageRepository.SaveChangesAsync();

            var responseDto = new UploadImageDto()
            {
                ImageUrl = imageUrl
            };

            return responseDto;
        }

        public async Task<UploadImageDto> SaveImageUrlToDatabase(
            string imageUrl, UserProfileImage profileImage, UserHomePageImage homePageImage)
        {
            if (profileImage != null)
            {
                await this.userProfileImageRepository.AddAsync(profileImage);
                await this.userProfileImageRepository.SaveChangesAsync();
            }
            else
            {
                await this.userHomePageRepository.AddAsync(homePageImage);
                await this.userHomePageRepository.SaveChangesAsync();
            }

            var responseDto = new UploadImageDto()
            {
                ImageUrl = imageUrl
            };

            return responseDto;
        }
        public async Task<ImageUploadResult> UploadHomePageImageAsync(IFormFile file)
        {
            int heigth = 1920;
            int width = 1280;

            ImageUploadResult uploadResult = await UploadImageToCloudinary(file, heigth, width);

            return uploadResult;
        }
        public async Task<ImageUploadResult> UploadProfileImageAsync(IFormFile file)
        {
            int heigth = 600;
            int width = 600;

            ImageUploadResult uploadResult = await UploadImageToCloudinary(file, heigth, width);

            return uploadResult;
        }

        private async Task<ImageUploadResult> UploadImageToCloudinary(IFormFile file, int heigth, int width)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation()
                    .Height(heigth).Width(width)
                };
                uploadResult = await cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }
    }
}
