namespace Portfolio.API.Services.Contracts
{
    using Portfolio.API.Dtos.GitHubApiDtos;
    public interface IGitHubApiService
    {
        /// <summary>
        /// Utilizing the GitHub API, retrieve the list of programming languages employed by a username in their GitHub profile.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task GetUserProgrammingLanguages(string userId);

        /// <summary>
        /// Retrieving language usage percentages from the database.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<LanguageStatsDto>> GetPercentageOfUseOnAllLanguages(string userId);

        /// <summary>
        /// Making an API call to the GitHub API to check the existence of an account by username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task HasUserAccountInGitHub(string username);
    }
}
