using Microsoft.AspNetCore.Identity;
using Portfolio.API.Data.Models;
using Portfolio.API.Dtos.UsersProfileDtos;
using Portfolio.API.Services.Dtos;
using Portfolio.Data.Repositories;

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
        public async Task<AboutUser> PersonalizeAboutUserAsync(AboutUserDto model, string userId)
        {

            var userAbout = new AboutUser()
            {
                About = model.About,
                Age = model.Age,
                City = model.City,
                Country = model.Country,
                Education = model.Education,
                PhoneNumber = model.PhoneNumber,
                UserId = userId
            };

            await usersAboutRepository.AddAsync(userAbout);
            await usersAboutRepository.SaveChangesAsync();

            return userAbout;
        }
    }
}
