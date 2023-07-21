namespace Portfolio.API.Services.AccountService
{
    using Microsoft.AspNetCore.Identity;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Services.Dtos;
    using Portfolio.API.Services.Dtos.AccountDtos;
    using Portfolio.API.Services.Models;
    public interface IAccountsService
    {
        /// <summary>
        ///  Authenticates the user. Login method is with password.
        /// </summary>
        /// <param name="user"> Contains the user's password and email address </param>
        /// <returns> A model of the user metadata(Id, UserName, Email, Password, AccessToken, RefreshToken) </returns>
        Task<ApplicationUserLoginResponseDto> AuthenticateUser(ApplicationUserLoginDto user);

        /// <summary>
        ///  Registers the user.
        /// </summary>
        /// <param name="user"> Contains the user's required register parameters(UserName, Email, Password, ConfirmPassword) </param>
        /// <returns> Metadata about the register process. It determines if the registration was successful </returns>
        Task<IdentityResult> RegisterUser(ApplicationUserRegisterDto applicationUser);

        /// <summary>
        ///  Gets a user from the database using his email address 
        /// </summary>
        /// <param name="email"> The user's email address </param>
        /// <returns> The user </returns>
        Task<ApplicationUser> GetUserByEmail(string email);
    }
}
