namespace Portfolio.API.Services.Contracts
{
    using Portfolio.API.Data.Models;
    using Portfolio.API.Dtos.ProjectsDto;
    using Portfolio.API.Dtos.UsersProfileDtos;

    public interface IProjectService
    {
        /// <summary>
        /// Add project full description.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<Project> AddProjectDetailsAsync(ProjectDto model, int projectId);

        /// <summary>
        /// Get all user's project images.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<ProjectMainImageDto>> GetAllProjectImagesAsync(string userId);

        /// <summary>
        /// Get current project by id.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<ProjectDto> GetProjectByIdAsync(int projectId);

        /// <summary>
        /// Get project image by project id.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task<string> GetProjectDetailsImageUrlAsync(int projectId);

        /// <summary>
        /// Delete current project by his identifier.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        Task DeleteProjectByIdAsync(int projectId);

        /// <summary>
        /// Edit project's information.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task EditProjectInformation(ProjectDto model);
    }
}
