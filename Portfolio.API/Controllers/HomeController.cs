namespace Portfolio.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Services.Contracts;

    [Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IImagesService imagesService;
        public HomeController(IImagesService imagesService)
        {
            this.imagesService = imagesService;
        }

        [HttpPost]
        [Route("upload-profile-image")]
        public async Task<IActionResult> UploadProfileImage(IFormFile file, [FromQuery] string userId)
        {
            var result = await this.imagesService.UploadProfileImageAsync(file, userId);

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

            var responseDto = await this.imagesService.SaveImageUrlToDatabaseAsync(profilePictureUrl, userProfileImage, userId);

            return Ok(responseDto);
        }

        [HttpPost]
        [Route("upload-homepage-image")]
        public async Task<IActionResult> UploadHomePageImage(IFormFile file, [FromQuery] string userId)
        {
            var result = await this.imagesService.UploadHomePageImageAsync(file, userId);

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

            var responseDto = await this.imagesService.SaveImageUrlToDatabaseAsync(profileHomeUrl, userHomePageImage, userId);

            return Ok(responseDto);
        }

        [HttpGet]
        [Route("get-profile-image/{userId}")]
        public async Task<IActionResult> GetUserProfileImage(string userId)
        {
            try
            {
                var imageUrl = await this.imagesService.GetUserProfileImageUrlAsync(userId);
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
                var imageUrl = await this.imagesService.GetUserHomePageImageUrlAsync(userId);
                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        [Route("edit-profile-image")]
        public async Task<IActionResult> EditProfileImage(IFormFile file, [FromQuery] string userId)
        {
            var result = await this.imagesService.UploadProfileImageAsync(file, userId);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var userProfileImage = new UserImage()
            {
                ProfileImageUrl = result.SecureUrl.AbsoluteUri,
                UserId = userId
            };

            string profileImageUrl = result.Url.AbsoluteUri;

            var response = await this.imagesService.EditImageUrlInDatabaseAsync(profileImageUrl, userProfileImage, userId);

            return Ok(response);
        }

        [HttpPut]
        [Route("edit-home-image")]
        public async Task<IActionResult> EditHomeImage(IFormFile file, [FromQuery] string userId)
        {
            var result = await this.imagesService.UploadHomePageImageAsync(file, userId);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var userHomeImageUrl = new UserImage()
            {
                HomePageImageUrl = result.SecureUrl.AbsoluteUri,
                UserId = userId
            };

            string homeImageUrl = result.Url.AbsoluteUri;

            var response = await this.imagesService.EditImageUrlInDatabaseAsync(homeImageUrl, userHomeImageUrl, userId);

            return Ok(response);
        }
    }
}
