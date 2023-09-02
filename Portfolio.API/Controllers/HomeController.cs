namespace Portfolio.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Services.Contracts;
    using System.Security.Claims;

    [Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IImagesService imageService;
        public HomeController(IImagesService imageService)
        {
            this.imageService = imageService;
        }

        [HttpPost]
        [Route("upload-profile-image")]
        public async Task<IActionResult> UploadProfileImage(IFormFile file, [FromQuery] string userId)
        {
            var result = await this.imageService.UploadProfileImageAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var userProfileImage = new UserImage()
            {
                ProfileImageUrl = result.SecureUrl.AbsoluteUri,
                UserId = userId
            };

            string profilePictureUrl = result.Url.AbsoluteUri;

            var responseDto = await this.imageService.SaveImageUrlToDatabase(profilePictureUrl, userProfileImage, userId);

            return Ok(responseDto);
        }

        [HttpPost]
        [Route("upload-homepage-image")]
        public async Task<IActionResult> UploadHomePageImage(IFormFile file, [FromQuery] string userId)
        {
            var result = await this.imageService.UploadHomePageImageAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var userHomePageImage = new UserImage()
            {
                HomePageImageUrl = result.SecureUrl.AbsoluteUri,
                UserId = userId
            };

            string profileHomeUrl = result.Url.AbsoluteUri;

            var responseDto = await this.imageService.SaveImageUrlToDatabase(profileHomeUrl, userHomePageImage, userId);

            return Ok(responseDto);
        }

        [HttpGet]
        [Route("get-profile-image/{userId}")]
        public async Task<IActionResult> GetUserProfileImage(string userId)
        {
            try
            {
                var imageUrl = await this.imageService.GetUserProfileImageUrlAsync(userId);
                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-home-page-image/{userId}")]
        public async Task<IActionResult> GetUserHomePageImage(string userId)
        {
            try
            {
                var imageUrl = await this.imageService.GetUserHomePageImageUrlAsync(userId);
                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
