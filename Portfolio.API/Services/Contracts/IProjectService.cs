namespace Portfolio.API.Services.Contracts
{
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.ProjectsDto;
    public interface IProjectService
    {
        Task<Project> AddProjectDetails(ProjectDto model, int projectId);

        Task<IEnumerable<ProjectMainImageDto>> GetAllProjectImages(string userId);
    }
}
