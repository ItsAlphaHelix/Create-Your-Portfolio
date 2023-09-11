namespace Portfolio.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.OpenApi.Models;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.ProjectsDto;
    using Portfolio.API.Services.Contracts;
    using SendGrid.Helpers.Errors.Model;

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
        [Route("upload-project-main-image")]
        public async Task<IActionResult> UploadMainImage(IFormFile file, [FromQuery] string userId)
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

            string projectMainImageUrl = result.Url.AbsoluteUri;

            var responseDto = await this.imagesService.SaveProjectImageToDatabase(projectMainImageUrl, projectImage, 0);

            return Ok(responseDto);
        }

        [HttpPost]
        [Route("upload-project-details-image")]
        public async Task<IActionResult> UploadDetailsImage(IFormFile file, [FromQuery] int projectId)
        {
            var result = await this.imagesService.UploadProjectDetailsImageAsync(file);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var projectImage = new Project()
            {
                ProjectDetailsImageUrl = result.SecureUrl.AbsoluteUri,
            };

            string projectDetailsImageUrl = result.Url.AbsoluteUri;

            var responseDto = await this.imagesService.SaveProjectImageToDatabase(projectDetailsImageUrl, projectImage, projectId);

            return Ok(responseDto);
        }

        [HttpPost]
        [Route("add-project/{projectId}")]
        public async Task<IActionResult> AddProject([FromBody] ProjectDto project, int projectId)
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

        [HttpGet]
        [Route("get-project/{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
             var result = await this.projectService.GetProjectById(id);          
             return Ok(result);
        }

        [HttpGet]
        [Route("get-project-details-image")]
        public async Task<IActionResult> GetProjectDetailsImage([FromQuery] int projectId)
        {
            try
            {
                string imageUrl = await this.imagesService.GetProjectDetailsImageUrlAsync(projectId);

                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
