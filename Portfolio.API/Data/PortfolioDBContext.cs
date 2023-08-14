namespace Portfolio.API.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Portfolio.API.Data.Models;
    using System.Reflection.Emit;

    public class PortfolioDBContext : IdentityDbContext<ApplicationUser>
    {
        public PortfolioDBContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public DbSet<UserProfileImage> UserProfileImages { get; set; }

        public DbSet<UserHomePageImage> UserHomePageImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
