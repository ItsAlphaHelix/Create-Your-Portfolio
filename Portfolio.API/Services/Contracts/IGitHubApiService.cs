using Portfolio.API.Dtos.GitHubApiDtos;

namespace Portfolio.API.Services.Contracts
{
    public interface IGitHubApiService
    {
        /// <summary>
        /// Utilizing the GitHub API, retrieve the list of programming languages employed by a user in their GitHub profile.
        /// </summary>
        /// <returns></returns>
        Task GetUserProgrammingLanguages();

        Task<IEnumerable<LanguagePercentage>> GetPercentageOfUseOnAllLanguages(string userId);
    }
}
