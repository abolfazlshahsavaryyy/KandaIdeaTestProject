using Microsoft.EntityFrameworkCore;
using AgriculturalLandManagement.Models;
using AgriculturalLandManagement.Data.Configurations;

namespace AgriculturalLandManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Land> Lands { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LandConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
