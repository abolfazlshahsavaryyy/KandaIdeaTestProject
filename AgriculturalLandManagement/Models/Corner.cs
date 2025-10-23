using System.ComponentModel.DataAnnotations;
namespace AgriculturalLandManagement.Models;
public class Corner
{
    public int Id { get; set; }

    [Required]
    public double Latitude { get; set; }

    [Required]
    public double Longitude { get; set; }

    // Foreign key to Land
    public int LandId { get; set; }
    public DateTime CreatedAt{ get; set; }

    // Navigation property
    public Land Land { get; set; }

    // One-to-one navigation to CornerImage
    public CornerImage Image { get; set; }
}
