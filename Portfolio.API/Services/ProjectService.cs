namespace Portfolio.API.Services
{
    using Microsoft.EntityFrameworkCore;
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.ProjectsDto;
    using Portfolio.API.Services.Contracts;
    using Portfolio.Data.Repositories;

    public class ProjectService : IProjectService
    {
        private readonly IRepository<Project> projectsRepository;

        public ProjectService(IRepository<Project> projectsRepository)
        {
            this.projectsRepository = projectsRepository;
        }
        public async Task<Project> AddProjectDetails(ProjectDto model, int projectId)
        {
            var project = await this.projectsRepository
                .All()
                .FirstOrDefaultAsync(x => x.Id == projectId);

            project.Name = model.Name;
            project.DeploymentUrl = model.DeploymentUrl;
            project.GitHubUrl = model.GitHubUrl;
            project.Environment = model.Environment;
            project.Category = model.Category;
            //project.Images.Add()
            await this.projectsRepository.SaveChangesAsync();

            return project; 
        }
    }
}
