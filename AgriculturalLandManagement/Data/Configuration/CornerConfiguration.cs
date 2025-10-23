using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AgriculturalLandManagement.Models;

public class CornerConfiguration : IEntityTypeConfiguration<Corner>
{
    public void Configure(EntityTypeBuilder<Corner> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Latitude)
            .IsRequired();

        builder.Property(c => c.Longitude)
            .IsRequired();

        builder.HasOne(c => c.Land)
            .WithMany(l => l.Corners)
            .HasForeignKey(c => c.LandId);

        builder.HasOne(c => c.Image)
            .WithOne(i => i.Corner)
            .HasForeignKey<CornerImage>(i => i.CornerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
