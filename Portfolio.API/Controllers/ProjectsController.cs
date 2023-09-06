namespace Portfolio.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.OpenApi.Models;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.ProjectsDto;
    using Portfolio.API.Services.Contracts;

    [Route("api/projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly IImagesService imagesService;
        private readonly IProjectService projectService;
        public ProjectsController(IImagesService imagesService, IProjectService projectService)
        {
            this.imagesService = imagesService;
            this.projectService = projectService;
        }

        [HttpPost]
        [Route("upload-project-image")]
        public async Task<IActionResult> UploadImage(IFormFile file, [FromQuery] string userId)
        {
            var result = await this.imagesService.UploadProjectMainImageAsync(file, userId);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var projectImage = new Project()
            {
                MainImageUrl = result.SecureUrl.AbsoluteUri,
                UserId = userId
            };

            string aboutImageUrl = result.Url.AbsoluteUri;

            var responseDto = await this.imagesService.SaveProjectImageToDatabase(aboutImageUrl, projectImage);

            return Ok(responseDto);
        }

        [HttpPost]
        [Route("add-project")]
        public async Task<IActionResult> AddProject([FromQuery] int projectId, [FromBody] ProjectDto project)
        {
            var result = await this.projectService.AddProjectDetails(project, projectId);

            return Ok(result);
        }

        [HttpGet]
        [Route("get-all-project-images")]
        public async Task<IActionResult> GetAllProjectImages([FromQuery] string userId)
        {
            var result = await this.projectService.GetAllProjectImages(userId);

            return Ok(result);
        }
    }
}
