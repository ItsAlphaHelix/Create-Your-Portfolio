namespace Portfolio.API.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Project
    {
        [Key]
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? MainImageUrl { get; set; }

        public string? ProjectDetailsImageUrl { get; set; }

        public string? DeploymentUrl { get; set; }

        public string? GitHubUrl { get; set; }

        public string? Environment { get; set; }

        public string? Category { get; set; }


        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
