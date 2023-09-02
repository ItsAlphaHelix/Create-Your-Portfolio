namespace Portfolio.API.Services
{
    using CloudinaryDotNet;
    using CloudinaryDotNet.Actions;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Conventions;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.ImagesDtos;
    using Portfolio.API.Services.Contracts;
    using Portfolio.Data.Repositories;

    public class ImagesService : IImagesService
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
            var user = await userImagesRepository.AllAsNoTracking()
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
            var user = await userImagesRepository.AllAsNoTracking()
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

        public async Task<UploadImageDto> EditImageInDatabase(string imageUrl, UserImage image, string userId)
        {
            var imageForEdit = await this.userImagesRepository
                 .All()
                 .FirstOrDefaultAsync(x => x.UserId == userId && x.AboutImageUrl != null);

            var responseDto = new UploadImageDto()
            {
                ImageUrl = imageUrl
            };

            if (imageForEdit == null)
            {
                await userImagesRepository.AddAsync(image);
                await userImagesRepository.SaveChangesAsync();

                return responseDto;
            }

            imageForEdit.AboutImageUrl = responseDto.ImageUrl;
            await this.userImagesRepository.SaveChangesAsync();

            return responseDto;
        }
        public async Task<UploadImageDto> SaveImageUrlToDatabase(
            string imageUrl, UserImage image, string userId)
        {
            var userImage = await this.userImagesRepository
                .All()
                .FirstOrDefaultAsync(x => x.UserId == userId);

            var responseDto = new UploadImageDto()
            {
                ImageUrl = imageUrl
            };

            if (userImage != null)
            {
                if (imageUrl.Contains("about"))
                {
                    userImage.AboutImageUrl = imageUrl;
                }
                else if (imageUrl.Contains("home"))
                {
                    userImage.HomePageImageUrl = imageUrl;
                }
                else if (imageUrl.Contains("profile"))
                {
                    userImage.ProfileImageUrl = imageUrl;
                }

                await userImagesRepository.SaveChangesAsync();

                return responseDto;
            }

            await userImagesRepository.AddAsync(image);
            await userImagesRepository.SaveChangesAsync();

            return responseDto;
        }
        public async Task<ImageUploadResult> UploadHomePageImageAsync(IFormFile file)
        {
            int heigth = 1280;
            int width = 1920;
            string publicId = "home";

            ImageUploadResult uploadResult = await UploadImageToCloudinary(file, heigth, width, publicId);

            return uploadResult;
        }
        public async Task<ImageUploadResult> UploadProfileImageAsync(IFormFile file)
        {
            int heigth = 600;
            int width = 600;
            string publicId = "profile";

            ImageUploadResult uploadResult = await UploadImageToCloudinary(file, heigth, width, publicId);

            return uploadResult;
        }

        public async Task<ImageUploadResult> UploadAboutImageAsync(IFormFile file)
        {
            int heigth = 600;
            int width = 600;
            string publicId = "about";
            ImageUploadResult uploadResult = await UploadImageToCloudinary(file, heigth, width, publicId);

            return uploadResult;
        }
        private async Task<ImageUploadResult> UploadImageToCloudinary(IFormFile file, int heigth, int width, string publicId)
        {
            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    PublicId = publicId,
                    Transformation = new Transformation()
                    .Height(heigth).Width(width)
                };
                uploadResult = await cloudinary.UploadAsync(uploadParams);
            }

            return uploadResult;
        }

        public async Task<string> GetAboutImageUrlAsync(string userId)
        {
            var user = await userImagesRepository.AllAsNoTracking()
                .Where(x => x.UserId == userId && x.AboutImageUrl != null)
                .Select(x => new UploadImageDto()
                {
                    ImageUrl = x.AboutImageUrl
                })
                .FirstOrDefaultAsync();

            return user.ImageUrl;
        }
    }
}
