namespace Portfolio.API.Services.PhotoService
{
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.ImagesDtos;
    using Portfolio.Data.Repositories;

    public class ImagesService: IImagesService
    {
        private readonly Cloudinary cloudinary;
        private readonly IRepository<UserImage> userImagesRepository;
        public ImagesService(
            IConfiguration configuration,
            IRepository<UserImage> userImagesRepository)
        {
            Account account = new Account(
                configuration["CloudinarySettings:Name"],
                configuration["CloudinarySettings:ApiKey"],
                configuration["CloudinarySettings:SecretKey"]);

            cloudinary = new Cloudinary(account);
            this.userImagesRepository = userImagesRepository;
        }

        public async Task<string> GetUserHomePageImageUrlAsync(string userId)
        {
            var user = await this.userImagesRepository.AllAsNoTracking()
                .Where(x => x.UserId == userId && x.HomePageImageUrl != null)
                .Select(x => new UploadImageDto()
                {
                    ImageUrl = x.HomePageImageUrl

                }).FirstOrDefaultAsync();

            if (user == null || string.IsNullOrEmpty(user.ImageUrl))
            {
                throw new Exception("You currently do not have a home image. To enhance your portfolio, consider uploading your own image. To upload the image, click on the window displaying your name and job title.");
            }
            
            return user.ImageUrl;
        }

        public async Task<string> GetUserProfileImageUrlAsync(string userId)
        {
            var user = await this.userImagesRepository.AllAsNoTracking()
                .Where(x => x.UserId == userId && x.ProfileImageUrl != null)
                .Select(x => new UploadImageDto()
                {
                    ImageUrl = x.ProfileImageUrl
                })
                .FirstOrDefaultAsync();

            if (user == null || string.IsNullOrEmpty(user.ImageUrl))
            {
                throw new Exception("You currently do not have a profile image. To enhance your portfolio, consider uploading your own image. To upload the image, click on the window displaying custom image 600X600.");
            }

            return user.ImageUrl;
        }

        public async Task<UploadImageDto> SaveImageUrlToDatabase(
            string imageUrl, UserImage image)
        {
            await this.userImagesRepository.AddAsync(image);
            await this.userImagesRepository.SaveChangesAsync();

            var responseDto = new UploadImageDto()
            {
                ImageUrl = imageUrl
            };

            return responseDto;
        }
        public async Task<ImageUploadResult> UploadHomePageImageAsync(IFormFile file)
        {
            int heigth = 1280;
            int width = 1920;

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

        public async Task<ImageUploadResult> UploadAboutImageAsync(IFormFile file)
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

        public async Task<string> GetAboutImageUrlAsync(string userId)
        {
            var user = await this.userImagesRepository.AllAsNoTracking()
                .Where(x => x.UserId == userId && x.AboutImageUrl != null)
                .Select(x => new UploadImageDto()
                {
                    ImageUrl = x.AboutImageUrl
                })
                .FirstOrDefaultAsync();

            //if (user == null || string.IsNullOrEmpty(user.ImageUrl))
            //{
            //    throw new Exception("You currently do not have a profile image. To enhance your portfolio, consider uploading your own image. To upload the image, click on the window displaying custom image 600X600.");
            //}

            return user.ImageUrl;
        }
    }
}
