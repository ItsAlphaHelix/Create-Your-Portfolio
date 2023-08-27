using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfolio.API.Data.Models;
using Portfolio.API.Dtos.UsersProfileDtos;
using Portfolio.Data.Repositories;
using SendGrid.Helpers.Errors.Model;

namespace Portfolio.API.Services.UsersProfileService
{
    public class UsersProfileService : IUsersProfileService
    {
        private readonly IRepository<ApplicationUser> usersRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IRepository<AboutUser> usersAboutRepository;
        public UsersProfileService(
            IRepository<ApplicationUser> usersRepository,
            UserManager<ApplicationUser> userManager,
            IRepository<AboutUser> usersAboutRepository)
        {
            this.usersRepository = usersRepository;
            this.userManager = userManager;
            this.usersAboutRepository = usersAboutRepository;

        }
        public async Task PersonalizeAboutAsync(AboutUserDto model, string userId)
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

        public async Task<AboutUserResponseDto> GetAboutAsync(string userId)
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

        public async Task<AboutUserDto> GetEditAboutInformation(int aboutId)
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
        public async Task EditAboutInformationAsync(AboutUserDto model)
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
    }
}
