namespace Portfolio.API.Services
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.GitHubApiDtos;
    using Portfolio.API.Services.Contracts;
    using Portfolio.Data.Repositories;
    using SendGrid.Helpers.Errors.Model;
    using System.Collections.Generic;
    using System.Net.Http.Headers;

    //{"C#", 250000},
    //            { "T-Sql", 100000 },
    //            { "Html", 50000 },
    //            { "Css", 20000},
    //            { "Typescript", 2000000 },
    //            { "Python", 100000 },
    //            { "Java", 5900},
    //            { "Vau.js", 90000}
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

            this.client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("portfolio", "1"));
        }

        public async Task HasUserAccountInGitHubAsync(string username)
        {
            using HttpResponseMessage userResponse = await client.GetAsync($"{BaseUrl}/users/{username}");

            if (!userResponse.IsSuccessStatusCode)
            {
                if ((int)userResponse.StatusCode == 404)
                {
                    throw new NotFoundException("Invalid github username.");
                }
            }
        }

        public async Task<IEnumerable<LanguageStatsDto>> GetPercentageOfUseOnAllLanguagesAsync(string userId)
        {
            var result = await this.userProgramLanguagesRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => new LanguageStatsDto()
                {
                    LanguageName = x.LanguageName,
                    PercentageOfUseLanguage = x.PercentageOfUseLanguage
                })
                .ToListAsync();

            return result;
        }
        public async Task GetUserProgrammingLanguagesAsync(string userId)
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

            var sortedLanguagesDictionary = languageUsage
                .OrderByDescending(pair => pair.Value)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            int countOfElementsForRemove = 0;

            if (sortedLanguagesDictionary.Count() > 6)
            {
                countOfElementsForRemove = sortedLanguagesDictionary.Count() - 6;
            }

            for (int i = 0; i < countOfElementsForRemove; i++)
            {
                var lastElement = sortedLanguagesDictionary.Last();
                sortedLanguagesDictionary.Remove(lastElement.Key);
            }

            await CalcPercentageOfUseProgrammingLanguageAsync(sortedLanguagesDictionary, userId);
        }

        /// <summary>
        /// This method initiates an API call to GitHub, fetching all repositories associated with the user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private async Task<RepositoryDto[]> GetUserRepositories(string username)
        {
            using HttpResponseMessage reposResponse = await client.GetAsync($"{BaseUrl}/users/{username}/repos");

            if (!reposResponse.IsSuccessStatusCode)
            {
                if ((int)reposResponse.StatusCode == 404)
                {
                    throw new NotFoundException("The user does not have repos.");
                }
            }
            using HttpContent content = reposResponse.Content;

            var repos = await content.ReadFromJsonAsync<RepositoryDto[]>();

            return repos;
        }

        /// <summary>
        /// This method calculates the usage percentage of programming languages and then stores the resulting data in the database.
        /// </summary>
        /// <param name="languageUsage"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task CalcPercentageOfUseProgrammingLanguageAsync(
            Dictionary<string,
            int> languageUsage, string userId)
        {
            var programmingLanguages = await this.userProgramLanguagesRepository
                .All()
                .Where(x => x.UserId == userId)
                .ToListAsync();

            var editDict = new Dictionary<string, double>();

            int totalBytes = 0;
            foreach (var language in languageUsage.Keys)
            {
                totalBytes += languageUsage[language];
            }

            double percentage = 0;
            int count = 0;

            foreach (var language in languageUsage.Keys)
            {
                int byteCount = languageUsage[language];
                percentage = (double)byteCount / totalBytes * 100;

                if (programmingLanguages.Count() == 0)
                {
                    var userProgrammingLanguages = new UserProgramLanguage()
                    {
                        LanguageName = language,
                        PercentageOfUseLanguage = percentage,
                        UserId = userId
                    };

                    await userProgramLanguagesRepository.AddAsync(userProgrammingLanguages);
                    continue;
                }

                editDict.Add(language, percentage);
            }

            UpdateProgrammingLanguagesAsync(programmingLanguages, editDict);

            await userProgramLanguagesRepository.SaveChangesAsync();
        }

        /// <summary>
        /// This method is executed only when there is existing data in the database. It serves the purpose of updating older records with new information.
        /// </summary>
        /// <param name="programmingLanguages"></param>
        /// <param name="editDict"></param>
        private void UpdateProgrammingLanguagesAsync(List<UserProgramLanguage> programmingLanguages, Dictionary<string, double> editDict)
        {
            if (editDict.Count() != 0)
            {
                int index = -1;
                foreach (var programmingLanguage in programmingLanguages)
                {
                    index++;
                    programmingLanguage.LanguageName = editDict.ElementAt(index).Key;
                    programmingLanguage.PercentageOfUseLanguage = editDict.ElementAt(index).Value;
                }
            }
        }
    }
}
