namespace Portfolio.API.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.ImagesDtos;
    using Portfolio.API.Dtos.UsersProfileDtos;
    using Portfolio.API.Services.Contracts;
    using Portfolio.Data.Repositories;
    using SendGrid.Helpers.Errors.Model;
    public class AboutMeService : IAboutMeService
    {
        private readonly IRepository<UserImage> userImagesRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<AboutUser> usersAboutRepository;
        public AboutMeService(
            UserManager<ApplicationUser> userManager,
            IRepository<AboutUser> usersAboutRepository,
            IRepository<UserImage> userImagesRepository)
        {
            this.userManager = userManager;
            this.usersAboutRepository = usersAboutRepository;
            this.userImagesRepository = userImagesRepository;
        }
        public async Task AddAboutUsersInformationAsync(AboutUserDto model, string userId)
        {
            var aboutUser = new AboutUser()
            {
                AboutMessage = model.AboutMessage,
                Age = model.Age,
                City = model.City,
                Country = model.Country,
                Education = model.Education,
                PhoneNumber = model.PhoneNumber,
                UserId = userId
            };

            await usersAboutRepository.AddAsync(aboutUser);
            await usersAboutRepository.SaveChangesAsync();
        }

        public async Task<AboutUserResponseDto> GetAboutUsersInformationAsync(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentNullException("The user shouln't be null");
            }

            var aboutResponse = await this.usersAboutRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == user.Id)
                .Select(x => new AboutUserResponseDto()
                {
                    Id = x.Id,
                    JobTitle = user.JobTitle,
                    AboutMessage = x.AboutMessage,
                    Age = x.Age,
                    City = x.City,
                    Country = x.Country,
                    Education = x.Education,
                    Email = user.Email,
                    PhoneNumber = x.PhoneNumber
                })
                .FirstOrDefaultAsync();

            if (aboutResponse == null)
            {
                throw new ArgumentNullException();
            }

            return aboutResponse;
        }

        public async Task<AboutUserDto> GetEditUsersAboutInformationAsync(int aboutId)
        {
            var result = await this.usersAboutRepository
                .AllAsNoTracking()
                .Where(x => x.Id == aboutId)
                .Select(x => new AboutUserDto()
                {
                    Id = x.Id,
                    AboutMessage = x.AboutMessage,
                    Age = x.Age,
                    City = x.City,
                    Country = x.Country,
                    Education = x.Education,
                    PhoneNumber = x.PhoneNumber
                })
                .FirstOrDefaultAsync();

            return result;
        }
        public async Task EditAboutUsersInformationAsync(AboutUserDto model)
        {
            var editAbout = await usersAboutRepository
                .All()
                .Where(x => x.Id == model.Id)
                .FirstOrDefaultAsync();

            if (editAbout == null)
            {
                throw new NotFoundException("About information not found.");
            }

            editAbout.AboutMessage = model.AboutMessage;
            editAbout.Age = model.Age;
            editAbout.City = model.City;
            editAbout.Country = model.Country;
            editAbout.Education = model.Education;
            editAbout.PhoneNumber = model.PhoneNumber;

            await usersAboutRepository.SaveChangesAsync();
        }

        public async Task<string> GetAboutImageUrlAsync(string userId)
        {
            var user = await userImagesRepository.AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => new UploadImageDto()
                {
                    ImageUrl = x.AboutImageUrl
                })
                .FirstOrDefaultAsync();

            return user.ImageUrl;
        }
    }
}
