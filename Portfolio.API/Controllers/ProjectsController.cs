namespace Portfolio.API.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.OpenApi.Models;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.ProjectsDto;
    using Portfolio.API.Services.Contracts;
    using Portfolio.Data.Repositories;
    using SendGrid.Helpers.Errors.Model;

    [Route("api/projects")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {
        private readonly ICloudinaryService cloudinaryService;
        private readonly IProjectService projectService;
        private readonly IDatabaseService databaseService;
        private readonly IRepository<Project> projectRepo;
        public ProjectsController(
            ICloudinaryService cloudinaryService,
            IProjectService projectService,
            IDatabaseService databaseService,
            IRepository<Project> projectRepo)
        {
            this.cloudinaryService = cloudinaryService;
            this.projectService = projectService;
            this.databaseService = databaseService;
            this.projectRepo = projectRepo;

        }

        [HttpPost]
        [Route("upload-project-main-image")]
        public async Task<IActionResult> UploadMainImage(IFormFile file, [FromQuery] string userId)
        {
            int heigth = 600;
            int width = 800;

            string uniqueIdentifier = Guid.NewGuid().ToString();
            string publicId = $"project_main_image_{uniqueIdentifier}";

            var result = await this.cloudinaryService.UploadImageToCloudinaryAsync(file, heigth, width, publicId);

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

            var responseDto = await this.databaseService.SaveProjectImageToDatabaseAsync(projectMainImageUrl, projectImage, 0);

            return Ok(responseDto);
        }

        [HttpPost]
        [Route("upload-project-details-image")]
        public async Task<IActionResult> UploadDetailsImage(IFormFile file, [FromQuery] int projectId)
        {
            int heigth = 600;
            int width = 1288;
            string uniqueIdentifier = Guid.NewGuid().ToString();
            string publicId = $"project_details_image_{uniqueIdentifier}";

            var result = await this.cloudinaryService.UploadImageToCloudinaryAsync(file, heigth, width, publicId);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var projectImage = new Project()
            {
               ProjectDetailsImageUrl = result.SecureUrl.AbsoluteUri,
            };

            string projectDetailsImageUrl = result.Url.AbsoluteUri;

            var responseDto = await this.databaseService.SaveProjectImageToDatabaseAsync(projectDetailsImageUrl, projectImage, projectId);

            return Ok(responseDto);
        }

        [HttpPost]
        [Route("add-project/{projectId}")]
        public async Task<IActionResult> AddProject([FromBody] ProjectDto project, int projectId)
        {
            var result = await this.projectService.AddProjectDetailsAsync(project, projectId);

            return Ok(result);
        }

        [HttpGet]
        [Route("get-all-project-images")]
        public async Task<IActionResult> GetAllProjectImages([FromQuery] string userId)
        {
            var result = await this.projectService.GetAllProjectImagesAsync(userId);

            return Ok(result);
        }

        [HttpGet]
        [Route("get-project/{projectId}")]
        public async Task<IActionResult> GetProject(int projectId)
        {
             var result = await this.projectService.GetProjectByIdAsync(projectId);          
             return Ok(result);
        }

        [HttpGet]
        [Route("get-project-details-image")]
        public async Task<IActionResult> GetProjectDetailsImage([FromQuery] int projectId)
        {
            try
            {
                string imageUrl = await this.projectService.GetProjectDetailsImageUrlAsync(projectId);

                return Ok(new { imageUrl });
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        [Route("edit-project-details-image/{projectId}")]
        public async Task<IActionResult> EditProjectDetailsImage(IFormFile file, int projectId)
        {
            var project = this.projectRepo.AllAsNoTracking()
                .FirstOrDefault(x => x.Id == projectId);
            int heigth = 600;
            int width = 1288;

            var splittedDetailsImageUrl = project.ProjectDetailsImageUrl.Split("/", StringSplitOptions.RemoveEmptyEntries);
            var splittedDetailsImagePublicId = splittedDetailsImageUrl[6].Split(".");
            string detailsImagePublicId = splittedDetailsImagePublicId[0];

            var result = await this.cloudinaryService.UploadImageToCloudinaryAsync(file, heigth, width, detailsImagePublicId);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var projectDetailsImage = new Project()
            {
                ProjectDetailsImageUrl = result.SecureUrl.AbsoluteUri,
            };

            string detailsImageUrl = result.Url.AbsoluteUri;

            var response = await this.databaseService.EditProjectImageToDatabaseAsync(detailsImageUrl, projectDetailsImage, projectId);

            return Ok(response);
        }

            [HttpPut]
        [Route("edit-project-main-image/{projectId}")]
        public async Task<IActionResult> EditProjectMainImage(IFormFile file, int projectId)
        {
            var project = this.projectRepo.AllAsNoTracking().FirstOrDefault(x => x.Id == projectId);
            int heigth = 600;
            int width = 800;

            var splittedMainImageUrl = project.MainImageUrl.Split("/", StringSplitOptions.RemoveEmptyEntries);
            var splittedMainImagePublicId = splittedMainImageUrl[6].Split(".");
            string mainImagePublicId = splittedMainImagePublicId[0];

            var result = await this.cloudinaryService.UploadImageToCloudinaryAsync(file, heigth, width, mainImagePublicId);

            if (result.Error != null)
            {
                return BadRequest(result.Error.Message);
            }

            var projectMainImage = new Project()
            {
                MainImageUrl = result.SecureUrl.AbsoluteUri,
            };

            string projectMainImageUrl = result.Url.AbsoluteUri;

            var response = await this.databaseService.EditProjectImageToDatabaseAsync(projectMainImageUrl, projectMainImage, projectId);

            return Ok(response);
        }

        [HttpDelete]
        [Route("delete-project/{projectId}")]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            await this.projectService.DeleteProjectByIdAsync(projectId);

            return Ok();
        }
    }
}
