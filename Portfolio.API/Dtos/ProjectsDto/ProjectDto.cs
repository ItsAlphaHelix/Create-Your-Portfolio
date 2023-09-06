namespace Portfolio.API.Dtos.ProjectsDto
{
    public class ProjectDto
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public string Environment { get; set; }

        public string DeploymentUrl { get; set; }

        public string GitHubUrl { get; set; }
    }
}
