namespace Portfolio.API.Services.AccountsService
{
    using Microsoft.AspNetCore.Identity;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Services.Dtos;
    using Portfolio.API.Services.Dtos.AccountsDtos;
    using Portfolio.API.Services.Models;
    public interface IAccountsService
    {
        /// <summary>
        ///  Authenticates the user. Login method is with password.
        /// </summary>
        /// <param name="user"> Contains the user's password and email address </param>
        /// <returns> A model of the user metadata(Id, UserName, Email, Password, AccessToken, RefreshToken) </returns>
        Task<ApplicationUserLoginResponseDto> AuthenticateUserAsync(ApplicationUserLoginDto user);

        /// <summary>
        ///  Registers the user.
        /// </summary>
        /// <param name="user"> Contains the user's required register parameters(UserName, Email, Password, ConfirmPassword) </param>
        /// <returns> Metadata about the register process. It determines if the registration was successful </returns>
        Task<IdentityResult> RegisterUserAsync(ApplicationUserRegisterDto applicationUser);

        /// <summary>
        ///  Gets a user from the database using his email address 
        /// </summary>
        /// <param name="email"> The user's email address </param>
        /// <returns> The user </returns>
        Task<ApplicationUser> GetUserByEmailAsync(string email);


        /// <summary>
        ///  Refreshes the JWT access token with the use of the user's refresh token
        /// </summary>
        /// <param name="refreshToken"> The user's refresh token </param>
        /// <param name="userId"> The user's Id </param>
        /// <returns> A model containing both Refresh and Access tokens </returns>
        Task<ApplicationUserTokensDto> RefreshAccessTokenAsync(string refreshToken, string userId);

        /// <summary>
        /// Getting the user by ID.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ApplicationUserDto> GetUserAsync(string userId);
    }
}
