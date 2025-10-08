using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AgriculturalLandManagement.Models;

namespace AgriculturalLandManagement.Data.Configurations
{
    public class LandConfiguration : IEntityTypeConfiguration<Land>
    {
        public void Configure(EntityTypeBuilder<Land> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.OwnerName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(l => l.Production)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(l => l.X1).IsRequired();
            builder.Property(l => l.Y1).IsRequired();
            builder.Property(l => l.X2).IsRequired();
            builder.Property(l => l.Y2).IsRequired();

            builder.Property(l => l.Area).IsRequired();
        }
    }
}
