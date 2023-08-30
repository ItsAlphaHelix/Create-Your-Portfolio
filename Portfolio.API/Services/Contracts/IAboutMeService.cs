namespace Portfolio.API.Services.Contracts
{
    using Portfolio.API.Dtos.UsersProfileDtos;
    public interface IAboutMeService
    {
        /// <summary>
        /// This method adds user's personal information to the database.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task AddAboutUsersInformationAsync(AboutUserDto model, string userId);

        /// <summary>
        /// This method retrieves personal information for user's from the database.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<AboutUserResponseDto> GetAboutUsersInformationAsync(string userId);

        /// <summary>
        /// This method retrieves user's personal information from the database for editing purposes.
        /// </summary>
        /// <param name="aboutId"></param>
        /// <returns></returns>
        Task<AboutUserDto> GetEditUsersAboutInformationAsync(int aboutId);

        /// <summary>
        ///  This method edits personal information for users.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task EditAboutUsersInformationAsync(AboutUserDto model);
    }
}
