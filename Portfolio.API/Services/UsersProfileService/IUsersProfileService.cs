namespace Portfolio.API.Services.UsersProfileService
{
    using CloudinaryDotNet.Actions;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.UsersProfileDtos;

    public interface IUsersProfileService
    {
        /// <summary>
        /// This method customizes the "about" information for the user;
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task PersonalizeAboutAsync(AboutUserDto model, string userId);

        /// <summary>
        /// This method retrieves from database "about" information for the user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<AboutUserResponseDto> GetAboutAsync(string userId);
    }
}
