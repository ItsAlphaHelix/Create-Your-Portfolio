namespace Portfolio.API.Controllers
{
    using CloudinaryDotNet.Actions;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.ImagesDtos;
    using Portfolio.API.Services.PhotoService;
    using Portfolio.Data.Repositories;
    using System.Security.Claims;
    using System.Text.RegularExpressions;

    [Route("api/user-profile")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly ICloudinaryService photoService;
        private readonly IRepository<UserImage> userImageRepository;

        public UserController(
            ICloudinaryService photoService,
            IRepository<UserImage> userImageRepository)
        {
            this.photoService = photoService;
            this.userImageRepository = userImageRepository;
        }

        [Route("upload-profile-image")]
        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var result = await this.photoService.UploadProfilePictureAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var photo = new UserImage()
            {
                ProfileImageUrl = result.SecureUrl.AbsoluteUri,
                UserId = userId
            };

            UploadImageDto responseDto = await SaveImageUrlToDatabase(result, photo);

            return Ok(responseDto);
        }

        private async Task<UploadImageDto> SaveImageUrlToDatabase(ImageUploadResult result, UserImage photo)
        {
            //Todo seperate logic for save to database;
            await this.userImageRepository.AddAsync(photo);
            await this.userImageRepository.SaveChangesAsync();

            string profilePictureUrl = result.Url.AbsoluteUri;

            var responseDto = new UploadImageDto()
            {
                ImageUrl = profilePictureUrl
            };
            return responseDto;
        }

        [Route("upload-homepage-image")]
        [HttpPost]
        public async Task<IActionResult> UploadHomePageImage(IFormFile file)
        {
            var result = await this.photoService.UploadHomePagePictureAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var photo = new UserImage()
            {
                HomePageImageUrl = result.SecureUrl.AbsoluteUri,
                UserId = userId
            };

            await this.userImageRepository.AddAsync(photo);
            await this.userImageRepository.SaveChangesAsync();

            var responseDto = new UploadImageDto()
            {
                ImageUrl = result.Url.AbsoluteUri
            };

            return Ok(responseDto);
        }

        [HttpGet("get-profile-image/{userId}")]
        public async Task<IActionResult> GetUserProfileImage(string userId)
        {
            try
            {
                var imageUrl = await this.photoService.GetUserProfilePictureUrlAsync(userId);
                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
