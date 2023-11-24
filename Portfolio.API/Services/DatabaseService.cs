namespace Portfolio.API.Services
{
    using Microsoft.EntityFrameworkCore;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.ImagesDtos;
    using Portfolio.API.Services.Contracts;
    using Portfolio.Data.Repositories;
    public class DatabaseService : IDatabaseService
    {

        private readonly IRepository<UserImage> userImagesRepository;
        private readonly IRepository<Project> projectRepository;

        public DatabaseService(
            IRepository<UserImage> userImagesRepository,
            IRepository<Project> projectRepository)
        {
            this.userImagesRepository = userImagesRepository;
            this.projectRepository = projectRepository;
        }
        public async Task<UploadImageDto> EditImageUrlInDatabaseAsync(string imageUrl, UserImage image, string userId)
        {
            var imageForEdit = await this.userImagesRepository
                 .All()
                 .FirstOrDefaultAsync(x => x.UserId == userId);

            var responseDto = new UploadImageDto()
            {
                ImageUrl = imageUrl
            };

            if (imageForEdit != null)
            {
                CheckAndEdit(imageUrl, imageForEdit);
                await userImagesRepository.SaveChangesAsync();
            }
            
            return responseDto;
        }

        public async Task<UploadImageDto> EditProjectImageToDatabaseAsync(string imageUrl, Project image, int projectId)
        {
            var imageForEdit = await this.projectRepository
                 .All()
                 .FirstOrDefaultAsync(x => x.Id == projectId);

            var responseDto = new UploadImageDto()
            {
                ImageUrl = imageUrl
            };

            if (imageForEdit != null)
            {
                CheckAndEditProjectImage(imageUrl, imageForEdit);
                await projectRepository.SaveChangesAsync();
            }

            return responseDto;
        }
        public async Task<UploadImageDto> SaveImageUrlToDatabaseAsync(string imageUrl, UserImage image, string userId)
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
                CheckAndEdit(imageUrl, userImage);

                await userImagesRepository.SaveChangesAsync();

                return responseDto;
            }

            await userImagesRepository.AddAsync(image);
            await userImagesRepository.SaveChangesAsync();

            return responseDto;
        }

        public async Task<UploadImageDto> SaveProjectImageToDatabaseAsync(string imageUrl, Project project, int projectId)
        {
            var projectImage = await this.projectRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == projectId);


            var responseDto = new UploadImageDto()
            {
                ImageUrl = imageUrl
            };

            if (projectImage != null)
            {
                projectImage.ProjectDetailsImageUrl = imageUrl;

                await projectRepository.SaveChangesAsync();

                return responseDto;
            }

            await this.projectRepository.AddAsync(project);
            await this.projectRepository.SaveChangesAsync();

            return responseDto;
        }

        /// <summary>
        /// This method checks the public IDs of images and edit them.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="imageForEdit"></param>
        private static void CheckAndEdit(string imageUrl, UserImage? imageForEdit)
        {
            if (imageUrl.Contains("about"))
            {
                imageForEdit.AboutImageUrl = imageUrl;
            }
            else if (imageUrl.Contains("home"))
            {
                imageForEdit.HomePageImageUrl = imageUrl;
            }
            else if (imageUrl.Contains("profile"))
            {
                imageForEdit.ProfileImageUrl = imageUrl;
            }
        }

        /// <summary>
        /// This method checks the public IDs of project images and edit them.
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <param name="imageForEdit"></param>
        private static void CheckAndEditProjectImage(string imageUrl, Project? imageForEdit)
        {
            if (imageUrl.Contains("project_main_image"))
            {
                imageForEdit.MainImageUrl = imageUrl;
            }
            else if (imageUrl.Contains("project_details_image"))
            {
                imageForEdit.ProjectDetailsImageUrl = imageUrl;
            }
        }
    }
}
