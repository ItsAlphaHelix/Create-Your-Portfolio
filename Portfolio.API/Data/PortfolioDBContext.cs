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

        public DbSet<UserImage> UserImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
           // builder.Entity<UserImage>()
           //.HasKey(x => new { x.Id, x.UserId });
            base.OnModelCreating(builder);
        }
    }
}
