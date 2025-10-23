using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AgriculturalLandManagement.Models;

public class CornerImageConfiguration : IEntityTypeConfiguration<CornerImage>
{
    public void Configure(EntityTypeBuilder<CornerImage> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.ImageData)
            .IsRequired();

        builder.HasOne(i => i.Corner)
            .WithOne(c => c.Image)
            .HasForeignKey<CornerImage>(i => i.CornerId);
    }
}
