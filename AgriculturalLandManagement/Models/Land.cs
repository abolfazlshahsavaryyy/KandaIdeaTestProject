using System.ComponentModel.DataAnnotations;

namespace AgriculturalLandManagement.Models
{
    public class Land
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string OwnerName { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Production { get; set; } = string.Empty;

    [Required]
    public double Area { get; set; }

    // Navigation property
    public ICollection<Corner> Corners { get; set; } = new List<Corner>();
}

}
