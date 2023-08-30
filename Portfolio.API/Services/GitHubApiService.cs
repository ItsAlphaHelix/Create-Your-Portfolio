namespace Portfolio.API.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.GitHubApiDtos;
    using Portfolio.API.Services.Contracts;
    using Portfolio.Data.Repositories;
    using System.Net.Http.Headers;
    using System.Security.Claims;

    public class GitHubApiService : IGitHubApiService
    {
        private const string BaseUrl = "https://api.github.com";
        private readonly HttpClient client = new HttpClient();
        private readonly IRepository<UserProgramLanguage> userProgramLanguagesRepository;
        private readonly UserManager<ApplicationUser> userManager;
        public GitHubApiService(
            IRepository<UserProgramLanguage> userProgramLanguagesRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.userProgramLanguagesRepository = userProgramLanguagesRepository;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<LanguagePercentage>> GetPercentageOfUseOnAllLanguages(string userId)
        {
            var result = await this.userProgramLanguagesRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => new LanguagePercentage()
                {
                    LanguageName = x.LanguageName,
                    PercentageOfUseLanguage = x.PercentageOfUseLanguage
                })
                .ToListAsync();

            return result;
        }

        public async Task GetUserProgrammingLanguages(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new ArgumentNullException("The user can not be null.");
            }

            var repos = await GetUserRepositories(user.UserName);

            var languageUsage = new Dictionary<string, int>();

            foreach (var repo in repos)
            {
                using HttpResponseMessage languagesResponse = await client.GetAsync(repo.LanguagesUrl);

                if (!languagesResponse.IsSuccessStatusCode)
                {
                    //return StatusCode((int)languagesResponse.StatusCode);
                }

                using HttpContent languageContent = languagesResponse.Content;

                var languagesStream = await languagesResponse.Content.ReadAsStreamAsync();

                var languages = await languageContent.ReadFromJsonAsync<Dictionary<string, int>>();

                foreach (var language in languages)
                {

                    if (languageUsage.ContainsKey(language.Key))
                    {
                        languageUsage[language.Key] += language.Value;
                    }
                    else
                    {
                        languageUsage[language.Key] = language.Value;
                    }
                }
            }

            var sortedDictionary = languageUsage
                .OrderByDescending(pair => pair.Value)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            int countOfElementsForRemove = 0;

            if (sortedDictionary.Count() > 6)
            {
                countOfElementsForRemove = sortedDictionary.Count() - 6;
            }

            for (int i = 0; i < countOfElementsForRemove; i++)
            {
                var lastElement = sortedDictionary.Last();
                sortedDictionary.Remove(lastElement.Key);
            }

            await SaveToDatabasePercentageOfUsesProgrammingLanguages(sortedDictionary, userId);
        }

        private async Task<RepositoryDto[]> GetUserRepositories(string username)
        {
           // client.DefaultRequestHeaders.Add("Authorization", "Bearer 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");
            client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("product", "1"));

            using HttpResponseMessage reposResponse = await client.GetAsync($"{BaseUrl}/users/{username}/repos");

            if (!reposResponse.IsSuccessStatusCode)
            {
                //return StatusCode((int)reposResponse.StatusCode);
            }

            using HttpContent content = reposResponse.Content;

            var repos = await content.ReadFromJsonAsync<RepositoryDto[]>();

            //if (repos.Length == 0)
            //{
            //    throw new HttpRequestException("The user does not have repos.");
            //}

            return repos;
        }

        private async Task SaveToDatabasePercentageOfUsesProgrammingLanguages(
            Dictionary<string,
            int> languageUsage, string userId)
        {
            int totalBytes = 0;
            foreach (var language in languageUsage.Keys)
            {
                totalBytes += languageUsage[language];
            }

            double percentage = 0;
            int count = 0;
            foreach (var language in languageUsage.Keys)
            {
                //count++;
                //var programLanguage = new ProgramLanguage()
                //{
                //    Id = count += 1,
                //    Name = language,
                //};

                int byteCount = languageUsage[language];
                percentage = (double)byteCount / totalBytes * 100;

                var userProgrammingLanguages = new UserProgramLanguage()
                {
                    LanguageName = language,
                    PercentageOfUseLanguage = percentage,
                    UserId = userId
                };

                await userProgramLanguagesRepository.AddAsync(userProgrammingLanguages);
            }

            await userProgramLanguagesRepository.SaveChangesAsync();
        }
    }
}
