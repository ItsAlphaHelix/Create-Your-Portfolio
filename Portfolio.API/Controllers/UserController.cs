namespace Portfolio.API.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.ImagesDtos;
    using Portfolio.API.Services.PhotoService;
    using Portfolio.Data.Repositories;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.Contracts;
    using System.Security.Claims;
    using System.Security.Principal;

    [Route("api/user-profile")]
    [ApiController]
    [Authorize]
    public class UserProfileController : ControllerBase
    {
        private readonly IPhotoService photoService;
        private readonly IRepository<UserImage> userImageRepository;

        public UserProfileController(
            IPhotoService photoService,
            IRepository<UserImage> userImageRepository)
        {
            this.photoService = photoService;
            this.userImageRepository = userImageRepository;
        }

        [Route("upload-image")]
        [HttpPost]
        public async Task<IActionResult> UploadProfileImage(IFormFile file)
        {
            var result = await this.photoService.UploadPhotoAsync(file);

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

            await this.userImageRepository.AddAsync(photo);
            await this.userImageRepository.SaveChangesAsync();

            var responseDto = new UploadImageDto()
            {
                ImageUrl = result.Url.AbsoluteUri
            };

            return Ok(responseDto);
        }

        [HttpGet("get-profile-image/{userId}")]
        public async Task<IActionResult> GetProfileImage(string userId)
        {
            try
            {
                var imageUrl = await this.photoService.GetUserProfileImageUrl(userId);
                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
