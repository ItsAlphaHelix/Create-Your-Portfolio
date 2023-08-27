namespace Portfolio.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.UsersProfileDtos;
    using Portfolio.API.Services.PhotoService;
    using Portfolio.API.Services.UsersProfileService;
    using Portfolio.Data.Repositories;
    using System.Security.Claims;

    [Route("api/users-profile")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IImagesService imageService;
        private readonly IUsersProfileService usersProfileService;
        private readonly IRepository<UserImage> userImagesRepository;

        public UsersController(IImagesService imageService, IUsersProfileService usersProfileService, IRepository<UserImage> userImagesRepository)
        {
            this.imageService = imageService;
            this.usersProfileService = usersProfileService;
            this.userImagesRepository = userImagesRepository;
        }

        [HttpPost]
        [Route("upload-profile-image")]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var result = await this.imageService.UploadProfileImageAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userProfileImage = new UserImage()
            {
                ProfileImageUrl = result.SecureUrl.AbsoluteUri,
                UserId = userId
            };

            string profilePictureUrl = result.Url.AbsoluteUri;

            var responseDto = await this.imageService.SaveImageUrlToDatabase(profilePictureUrl, userProfileImage);

            return Ok(responseDto);
        }

        [HttpPost]
        [Route("upload-homepage-image")]
        public async Task<IActionResult> UploadHomePageImage(IFormFile file)
        {
            var result = await this.imageService.UploadHomePageImageAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var userHomePageImage = new UserImage()
            {
                HomePageImageUrl = result.SecureUrl.AbsoluteUri,
                UserId = userId
            };

            string profileHomeUrl = result.Url.AbsoluteUri;

            var responseDto = await this.imageService.SaveImageUrlToDatabase(profileHomeUrl, userHomePageImage);

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

        [HttpPost]
        [Route("personalize-about")]
        public async Task<IActionResult> PersonalizeAbout([FromBody] AboutUserDto model, [FromQuery] string userId)
        {
            await this.usersProfileService.PersonalizeAboutAsync(model, userId);

            return Ok();
        }

        [HttpPost]
        [Route("upload-about-image")]
        public async Task<IActionResult> UploadAboutImage(IFormFile file, [FromQuery] string userId)
        {
            var result = await this.imageService.UploadAboutImageAsync(file);

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

            var responseDto = await this.imageService.SaveImageUrlToDatabase(aboutImageUrl, userAboutImage);

            return Ok(responseDto);
        }

        [HttpGet]
        [Route("get-about")]
        public async Task<IActionResult> GetAbout([FromQuery] string userId)
        {
            try
            {
                var response = await this.usersProfileService.GetAboutAsync(userId);

                return Ok(response);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("get-about-image/{userId}")]
        public async Task<IActionResult> GetAboutImage(string userId)
        {
            try
            {
                string imageUrl = await this.imageService.GetAboutImageUrlAsync(userId);

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
            var result = await this.usersProfileService.GetEditAboutInformation(aboutId);
            
            return Ok(result);
        }

        [HttpPut]
        [Route("edit-about")]
        public async Task<IActionResult> EditAboutInformation([FromBody] AboutUserDto model)
        {
            await this.usersProfileService.EditAboutInformationAsync(model);

            return Ok(new {id = model.Id});
        }
    }
}
