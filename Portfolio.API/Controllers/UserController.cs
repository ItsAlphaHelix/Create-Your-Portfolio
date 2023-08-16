namespace Portfolio.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Services.PhotoService;
    using System.Security.Claims;

    [Route("api/user-profile")]
    [ApiController]
    [Authorize]
    public class UserController: ControllerBase
    {
        private readonly IImageService imageService;

        public UserController(IImageService imageService)
        {
            this.imageService = imageService;
        }

        [Route("upload-profile-image")]
        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var result = await this.imageService.UploadProfileImageAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userProfileImage = new UserProfileImage()
            {
                ProfileImageUrl = result.SecureUrl.AbsoluteUri,
                UserId = userId
            };

            string profilePictureUrl = result.Url.AbsoluteUri;

            var responseDto = await this.imageService.SaveImageUrlToDatabase(profilePictureUrl, userProfileImage);

            return Ok(responseDto);
        }

        [Route("upload-homepage-image")]
        [HttpPost]
        public async Task<IActionResult> UploadHomePageImage(IFormFile file)
        {
            var result = await this.imageService.UploadHomePageImageAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userHomePageImage = new UserHomePageImage()
            {
                HomePageImageUrl = result.SecureUrl.AbsoluteUri,
                UserId = userId
            };

            string profilePictureUrl = result.Url.AbsoluteUri;

            var responseDto = await this.imageService.SaveImageUrlToDatabase(profilePictureUrl, null, userHomePageImage);

            return Ok(responseDto);
        }

        [HttpGet("get-profile-image/{userId}")]
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

        [HttpGet("get-home-page-image/{userId}")]
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
