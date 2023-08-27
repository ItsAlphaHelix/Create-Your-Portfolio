using Portfolio.API.Dtos.UsersProfileDtos;

namespace Portfolio.API.Services.Contracts
{
    public interface IAboutMeService
    {
        /// <summary>
        /// This method customizes the "about" information for the user;
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task AddAboutUsersInformationAsync(AboutUserDto model, string userId);

        /// <summary>
        /// This method retrieves from database "about" information for the user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<AboutUserResponseDto> GetAboutUsersInformationAsync(string userId);

        Task<AboutUserDto> GetEditUsersAboutInformationAsync(int aboutId);

        Task EditAboutUsersInformationAsync(AboutUserDto model);
    }
}
