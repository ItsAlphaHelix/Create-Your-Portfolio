namespace Portfolio.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore.Metadata.Internal;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.UsersProfileDtos;
    using Portfolio.API.Services.Contracts;
    using Portfolio.Data.Repositories;
    using SendGrid;
    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Text.Json;

    [Route("api/about-me")]
    [ApiController]
    public class AboutMeController : ControllerBase
    {
        private readonly IAboutMeService aboutMeService;
        private readonly IImagesService imagesService;
        private readonly IGitHubApi gitHubApiService;
        private readonly HttpClient client = new HttpClient();
        private readonly IRepository<UserProgramLanguage> userProgramLanguagesRepository;
        public AboutMeController(
            IAboutMeService usersProfileService,
            IImagesService imagesService,
            IGitHubApi gitHubApiService,
            IRepository<UserProgramLanguage> userProgramLanguagesRepository)
        {
            this.aboutMeService = usersProfileService;
            this.imagesService = imagesService;
            this.gitHubApiService = gitHubApiService;
            this.userProgramLanguagesRepository = userProgramLanguagesRepository;
        }

        [HttpPost]
        [Route("add-about")]
        public async Task<IActionResult> AddAboutInformation([FromBody] AboutUserDto model, [FromQuery] string userId)
        {
            await this.aboutMeService.AddAboutUsersInformationAsync(model, userId);

            return Ok();
        }

        [HttpPost]
        [Route("upload-about-image")]
        public async Task<IActionResult> UploadImage(IFormFile file, [FromQuery] string userId)
        {
            var result = await this.imagesService.UploadAboutImageAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var userAboutImage = new UserImage()
            {
                AboutImageUrl = result.SecureUrl.AbsoluteUri,
                UserId = userId
            };

            string aboutImageUrl = result.Url.AbsoluteUri;

            var responseDto = await this.imagesService.SaveImageUrlToDatabase(aboutImageUrl, userAboutImage);

            return Ok(responseDto);
        }

        [HttpGet]
        [Route("get-about")]
        public async Task<IActionResult> GetAboutUsersInformation([FromQuery] string userId)
        {
            try
            {
                var response = await this.aboutMeService.GetAboutUsersInformationAsync(userId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-about-image/{userId}")]
        public async Task<IActionResult> GetImage(string userId)
        {
            try
            {
                string imageUrl = await this.imagesService.GetAboutImageUrlAsync(userId);

                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-edit-about/{aboutId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetEditAboutInformation(int aboutId)
        {
            var result = await this.aboutMeService.GetEditUsersAboutInformationAsync(aboutId);

            return Ok(result);
        }

        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> EditAboutInformation([FromBody] AboutUserDto model)
        {
            await this.aboutMeService.EditAboutUsersInformationAsync(model);

            return Ok(new { id = model.Id });
        }

        [HttpGet]
        [Route("getUserRepo")]
        public async Task<IActionResult> getUserRepo()
        {
            this.client.DefaultRequestHeaders.Add("Authorization", "Bearer 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");
            this.client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("product", "1"));

            var reposResponse = await this.client.GetAsync("https://api.github.com/users/ItsAlphaHelix/repos");

            if (!reposResponse.IsSuccessStatusCode)
            {
                return StatusCode((int)reposResponse.StatusCode);
            }
            using HttpContent content = reposResponse.Content;

            var repos = await content.ReadFromJsonAsync<RepositoryDto[]>();

            var languageUsage = new Dictionary<string, int>();

            foreach (var repo in repos)
            {
                var languagesResponse = await client.GetAsync(repo.LanguagesUrl);
                if (!languagesResponse.IsSuccessStatusCode)
                {
                    return StatusCode((int)languagesResponse.StatusCode);
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
            return Ok(percentage.ToString("F1"));
        }
    }
}
