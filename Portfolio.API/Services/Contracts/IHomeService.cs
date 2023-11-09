namespace Portfolio.API.Services.Contracts
{
    public interface IHomeService
    {
        /// <summary>
        /// The method should retrieve the user's profile image from the database and return it.
        /// </summary>
        /// <returns></returns>
        Task<string> GetUserProfileImageUrlAsync(string userId);

        /// <summary>
        /// The method should retrieve the user's home page image from the database and return it.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GetUserHomePageImageUrlAsync(string userId);
    }
}
