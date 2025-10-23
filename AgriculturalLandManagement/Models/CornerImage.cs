using System.ComponentModel.DataAnnotations;

namespace AgriculturalLandManagement.Models;
public class CornerImage
{
    public int Id { get; set; }

    [Required]
    public byte[] ImageData { get; set; }

    // Foreign key to Corner
    public int CornerId { get; set; }

    // Navigation property
    public Corner Corner { get; set; }
}
