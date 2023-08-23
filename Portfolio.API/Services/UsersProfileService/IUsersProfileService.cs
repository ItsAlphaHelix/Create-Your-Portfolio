namespace Portfolio.API.Services.UsersProfileService
{
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.UsersProfileDtos;

    public interface IUsersProfileService
    {
        Task<AboutUser> PersonalizeAboutUserAsync(AboutUserDto model, string userId);
    }
}
