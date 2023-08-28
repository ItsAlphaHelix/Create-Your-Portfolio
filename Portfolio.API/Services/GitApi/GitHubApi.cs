using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Portfolio.API.Data.Models;
using Portfolio.API.Services.GitApi.Models;
using Portfolio.Data.Repositories;
using System.Net.Http.Headers;

namespace Portfolio.API.Services.GitApi
{
    public class GitHubApi : IGitHubApi
    {
        private const string BaseUrl = "https://api.github.com";
        private readonly HttpClient client = new HttpClient();
        private readonly IRepository<UserProgramLanguage> userProgramLanguagesRepository;
        public GitHubApi(IRepository<UserProgramLanguage> userProgramLanguagesRepository)
        {
            this.userProgramLanguagesRepository = userProgramLanguagesRepository;
        }

        public async Task GetUserProgrammingLanguages()
        {
            var repos = await GetUserRepositories();

            var languageUsage = new Dictionary<string, int>();

            foreach (var repo in repos)
            {
                var languagesResponse = await client.GetAsync(repo.LanguagesUrl);
                if (!languagesResponse.IsSuccessStatusCode)
                {
                    //return StatusCode((int)languagesResponse.StatusCode);
                }

                using HttpContent languageContent = languagesResponse.Content;

                var languagesStream = await languagesResponse.Content.ReadAsStreamAsync();

                var languages = await languageContent.ReadFromJsonAsync<Dictionary<string, int>>();

                foreach (var lang in languages)
                {
                    if (languageUsage.ContainsKey(lang.Key))
                    {
                        languageUsage[lang.Key] += lang.Value;
                    }
                    else
                    {
                        languageUsage[lang.Key] = lang.Value;
                    }
                }
            }
            await SaveToDatabasePercentageOfUsesProgrammingLanguages(languageUsage);
        }

        private async Task<RepositoryDto[]> GetUserRepositories()
        {
            this.client.DefaultRequestHeaders.Add("Authorization", "Bearer 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");
            this.client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("github", "1.1"));

            using HttpResponseMessage reposResponse = await this.client.GetAsync($"{BaseUrl}/users/ItsAlphaHelix/repos");

            if (!reposResponse.IsSuccessStatusCode)
            {
                //return StatusCode((int)reposResponse.StatusCode);
            }

            using HttpContent content = reposResponse.Content;

            var repos = await content.ReadFromJsonAsync<RepositoryDto[]>();

            return repos;
        }

        private async Task SaveToDatabasePercentageOfUsesProgrammingLanguages(Dictionary<string, int> languageUsage)
        {
            int totalBytes = 0;
            foreach (var language in languageUsage.Keys)
            {
                totalBytes += languageUsage[language];
            }
            double percentage = 0;
            foreach (var language in languageUsage.Keys)
            {
                int byteCount = languageUsage[language];
                percentage = ((double)byteCount / totalBytes) * 100;

                var userProgrammingLanguages = new UserProgramLanguage()
                {
                    LanguageName = language,
                    PercentageOfUseLanguage = percentage,
                    UserId = "aa35aabd-27b1-4041-a831-333931d205e4"
                };

                await this.userProgramLanguagesRepository.AddAsync(userProgrammingLanguages);
            }

            await this.userProgramLanguagesRepository.SaveChangesAsync();
        }
    }
}
