using Portfolio.API.Dtos.GitHubApiDtos;

namespace Portfolio.API.Services.Contracts
{
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
        Task<IEnumerable<LanguagePercentage>> GetPercentageOfUseOnAllLanguages(string userId);
    }
}
