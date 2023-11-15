namespace Portfolio.API.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Portfolio.API.Data.Models;

    public class PortfolioDBContext : IdentityDbContext<ApplicationUser>
    {
        public PortfolioDBContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<UserImage> UserImages { get; set; }

        public DbSet<AboutUser> AboutUsers { get; set; }

        public DbSet<UserProgramLanguage> UserProgramLanguages { get; set; }

        public DbSet<Project> Projects { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
