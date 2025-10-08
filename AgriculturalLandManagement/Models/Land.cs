using System.ComponentModel.DataAnnotations;

namespace AgriculturalLandManagement.Models
{
    public class Land : IValidatableObject
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string OwnerName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Production { get; set; } = string.Empty;

        [Required]
        public double X1 { get; set; }

        [Required]
        public double Y1 { get; set; }

        [Required]
        public double X2 { get; set; }

        [Required]
        public double Y2 { get; set; }

        [Required]
        public double Area { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            double expectedArea = Math.Abs(X1 - X2) * Math.Abs(Y1 - Y2);
            if (Math.Abs(expectedArea - Area) > 0.0001)
            {
                yield return new ValidationResult(
                    $"Area must be equal to |X1 - X2| * |Y1 - Y2|. Expected: {expectedArea}, but got: {Area}",
                    new[] { nameof(Area) });
            }
        }


    }
}
