namespace Portfolio.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.UsersProfileDtos;
    using Portfolio.API.Services.PhotoService;
    using Portfolio.API.Services.UsersProfileService;
    using System.Security.Claims;

    [Route("api/users-profile")]
    [ApiController]
    [Authorize]
    public class UsersController: ControllerBase
    {
        private readonly IImagesService imageService;
        private readonly IUsersProfileService usersProfileService;

        public UsersController(IImagesService imageService, IUsersProfileService usersProfileService)
        {
            this.imageService = imageService;
            this.usersProfileService = usersProfileService;
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

        [HttpPost("personalize-about")]
        public async Task<IActionResult> PersonalizeAboutUser([FromBody] AboutUserDto model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var aboutUser = await this.usersProfileService.PersonalizeAboutUserAsync(model, userId);

            return Ok(aboutUser);
        }
    }
}
