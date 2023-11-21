namespace Portfolio.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Services.Contracts;

    [Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ICloudinaryService cloudinaryService;
        private readonly IHomeService homeService;
        private readonly IDatabaseService databaseService;
        public HomeController(
            ICloudinaryService cloudinaryService,
            IHomeService homeService,
            IDatabaseService databaseService)
        {
            this.cloudinaryService = cloudinaryService;
            this.homeService = homeService;
            this.databaseService = databaseService;
        }

        [HttpPost]
        [Route("upload-profile-image")]
        public async Task<IActionResult> UploadProfileImage(IFormFile file, [FromQuery] string userId)
        {
            int heigth = 1280;
            int width = 1920;
            string publicId = "profile" + userId;

            var result = await this.cloudinaryService.UploadImageToCloudinaryAsync(file, heigth, width, publicId);

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

            var responseDto = await this.databaseService.SaveImageUrlToDatabaseAsync(profilePictureUrl, userProfileImage, userId);

            return Ok(responseDto);
        }

        [HttpPost]
        [Route("upload-homepage-image")]
        public async Task<IActionResult> UploadHomePageImage(IFormFile file, [FromQuery] string userId)
        {
            int heigth = 1280;
            int width = 1920;
            string publicId = "home" + userId;

            var result = await this.cloudinaryService.UploadImageToCloudinaryAsync(file, heigth, width, publicId);

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

            var responseDto = await this.databaseService.SaveImageUrlToDatabaseAsync(profileHomeUrl, userHomePageImage, userId);

            return Ok(responseDto);
        }

        [HttpGet]
        [Route("get-profile-image/{userId}")]
        public async Task<IActionResult> GetUserProfileImage(string userId)
        {
            try
            {
                var imageUrl = await this.homeService.GetUserProfileImageUrlAsync(userId);
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
                var imageUrl = await this.homeService.GetUserHomePageImageUrlAsync(userId);
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
            int heigth = 1280;
            int width = 1920;
            string publicId = "profile" + userId;

            var result = await this.cloudinaryService.UploadImageToCloudinaryAsync(file, heigth, width, publicId);

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

            var response = await this.databaseService.EditImageUrlInDatabaseAsync(profileImageUrl, userProfileImage, userId);

            return Ok(response);
        }

        [HttpPut]
        [Route("edit-home-image")]
        public async Task<IActionResult> EditHomeImage(IFormFile file, [FromQuery] string userId)
        {
            int heigth = 1280;
            int width = 1920;
            string publicId = "home" + userId;

            var result = await this.cloudinaryService.UploadImageToCloudinaryAsync(file, heigth, width, publicId);

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

            var response = await this.databaseService.EditImageUrlInDatabaseAsync(homeImageUrl, userHomeImageUrl, userId);

            return Ok(response);
        }
    }
}
