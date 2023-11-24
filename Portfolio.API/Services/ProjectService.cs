namespace Portfolio.API.Services
{
    using Microsoft.EntityFrameworkCore;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.ProjectsDto;
    using Portfolio.API.Dtos.UsersProfileDtos;
    using Portfolio.API.Services.Contracts;
    using Portfolio.Data.Repositories;
    using SendGrid.Helpers.Errors.Model;

    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> projectsRepository;
        public ProjectService(IRepository<Project> projectsRepository)
        {
            this.projectsRepository = projectsRepository;
        }
        public async Task<Project> AddProjectDetailsAsync(ProjectDto model, int projectId)
        {
            var project = await this.projectsRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == projectId);

            project.Name = model.Name;
            project.DeploymentUrl = model.DeploymentUrl;
            project.GitHubUrl = model.GitHubUrl;
            project.Environment = model.Environment;
            project.Category = model.Category;
            project.Description = model.Description;

            await this.projectsRepository.SaveChangesAsync();

            return project; 
        }

        public async Task<IEnumerable<ProjectMainImageDto>> GetAllProjectImagesAsync(string userId)
        {
            var projects = await this.projectsRepository
                .AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => new ProjectMainImageDto()
                {
                    projectId = x.Id,
                    Name = x.Name,
                    ProjectMainImageUrl = x.MainImageUrl
                })
                .ToListAsync();

            return projects;
        }

        public async Task<ProjectDto> GetProjectByIdAsync(int projectId)
        {
            var project = await this.projectsRepository
                .AllAsNoTracking()
                .Where(x => x.Id == projectId)
                .Select(x => new ProjectDto()
                {
                    Id = x.Id,
                    Name = x.Name,
                    DeploymentUrl = x.DeploymentUrl,
                    ProjectDetailsImageUrl = x.ProjectDetailsImageUrl,
                    Category = x.Category,
                    Description = x.Description,
                    GitHubUrl = x.GitHubUrl,
                    Environment = x.Environment
                })
                .FirstOrDefaultAsync();

            if (project.Environment == null && project.DeploymentUrl == null && project.Description == null
                && project.GitHubUrl == null && project.Category == null && project.Name == null)
            {
                throw new NotFoundException("The project wasn't found.");
            }

            return project;
        }

        public async Task<string> GetProjectDetailsImageUrlAsync(int projectId)
        {
            var projectImage = await this.projectsRepository
                .AllAsNoTracking()
                .Where(x => x.Id == projectId)
                .Select(x => new ProjectDetailsImageDto()
                {
                    ImageUrl = x.ProjectDetailsImageUrl
                })
                .FirstOrDefaultAsync();

            return projectImage.ImageUrl;
        }

        public async Task DeleteProjectByIdAsync(int projectId)
        {
            var project = this.projectsRepository.All().FirstOrDefault(x => x.Id == projectId);

            this.projectsRepository.Delete(project);
            await this.projectsRepository.SaveChangesAsync();
        }

        public async Task EditProjectInformation(ProjectDto model)
        {
            var editProject = await projectsRepository
                .All()
                .Where(x => x.Id == model.Id)
                .FirstOrDefaultAsync();

            if (editProject == null)
            {
                throw new NotFoundException("The project is not founded.");
            }

            editProject.Name = model.Name;
            editProject.Category = model.Category;
            editProject.Environment = model.Environment;
            editProject.DeploymentUrl = model.DeploymentUrl;
            editProject.Description = model.Description;
            editProject.GitHubUrl = model.GitHubUrl;

            await projectsRepository.SaveChangesAsync();
        }
    }
}
