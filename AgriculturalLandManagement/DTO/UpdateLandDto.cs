using System.ComponentModel.DataAnnotations;

namespace AgriculturalLandManagement.Dtos
{
    public class UpdateLandDto : IValidatableObject
    {
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

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (X1 == X2)
                yield return new ValidationResult(
                    "X1 and X2 cannot be equal. A land cannot be a vertical line.",
                    new[] { nameof(X1), nameof(X2) });

            if (Y1 == Y2)
                yield return new ValidationResult(
                    "Y1 and Y2 cannot be equal. A land cannot be a horizontal line.",
                    new[] { nameof(Y1), nameof(Y2) });
        }
    }
}
