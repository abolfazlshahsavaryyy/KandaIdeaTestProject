using Microsoft.EntityFrameworkCore;
using AgriculturalLandManagement.Models;
using AgriculturalLandManagement.Data.Configurations;


namespace AgriculturalLandManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Land> Lands { get; set; }
        public DbSet<Corner> Corners { get; set; }
        public DbSet<CornerImage> CornerImages { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LandConfiguration());
            modelBuilder.ApplyConfiguration(new CornerConfiguration());
            modelBuilder.ApplyConfiguration(new CornerImageConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
