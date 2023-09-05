namespace Portfolio.API.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.UsersProfileDtos;
    using Portfolio.API.Services;
    using Portfolio.API.Services.Contracts;
    using SendGrid.Helpers.Errors.Model;
    using System;

    [Route("api/about-me")]
    [ApiController]
    public class AboutMeController : ControllerBase
    {
        private readonly IAboutMeService aboutMeService;
        private readonly IImagesService imagesService;
        private readonly IGitHubApiService gitHubApiService;
        private readonly RateLimitCheckerService rateLimitCheckerService;
        public AboutMeController(
            IAboutMeService usersProfileService,
            IImagesService imagesService,
            IGitHubApiService gitHubApiService,
            RateLimitCheckerService rateLimitCheckerService)
        {
            this.aboutMeService = usersProfileService;
            this.imagesService = imagesService;
            this.gitHubApiService = gitHubApiService;
            this.rateLimitCheckerService = rateLimitCheckerService;
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
            var result = await this.imagesService.UploadAboutImageAsync(file, userId);

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

            var responseDto = await this.imagesService.SaveImageUrlToDatabaseAsync(aboutImageUrl, userAboutImage, userId);

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
        public async Task<IActionResult> GetEditAboutInformation(int aboutId)
        {
            var result = await this.aboutMeService.GetEditUsersAboutInformationAsync(aboutId);

            return Ok(result);
        }

        [HttpGet]
        [Route("get-github-repo-languages")]
        public async Task<IActionResult> getGitHubRepositoryLanguages([FromQuery] string userId)
        {
            try
            {
                var canInvoke = await this.rateLimitCheckerService.CheckRateLimitAsync(userId);

                if (canInvoke)
                {
                    await this.gitHubApiService.GetUserProgrammingLanguages(userId);
                }
                else
                {
                    return Forbid();
                }
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }

            return Ok();
        }

        [HttpGet]
        [Route("get-language-percentages")]
        public async Task<IActionResult> GetPercentagesOfLanguages([FromQuery] string userId)
        {
            var result = await this.gitHubApiService.GetPercentageOfUseOnAllLanguages(userId);

            return Ok(result);
        }

        [HttpPut]
        [Route("edit-about-image")]
        public async Task<IActionResult> EditAboutImage(IFormFile file, [FromQuery] string userId)
        {
            var result = await this.imagesService.UploadAboutImageAsync(file, userId);

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

            var response = await this.imagesService.EditImageUrlInDatabaseAsync(aboutImageUrl, userAboutImage, userId);

            return Ok(response);
        }


        [HttpPut]
        [Route("edit")]
        public async Task<IActionResult> EditAboutInformation([FromBody] AboutUserDto model)
        {
            await this.aboutMeService.EditAboutUsersInformationAsync(model);

            return Ok(new { id = model.Id });
        }
    }
}
