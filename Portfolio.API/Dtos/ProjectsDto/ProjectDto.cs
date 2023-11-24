namespace Portfolio.API.Dtos.ProjectsDto
{
    public class ProjectDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public string Environment { get; set; }

        public string DeploymentUrl { get; set; }

        public string? ProjectDetailsImageUrl { get; set; }

        public string Description { get; set; }

        public string GitHubUrl { get; set; }
    }
}
                                     