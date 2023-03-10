using System.ComponentModel.DataAnnotations;

namespace EFCore.Common;

public class DatabaseSettings : IValidatableObject
{
    public string ConnectionString { get; set; } = string.Empty;

    public string ConnectionStringTemplate { get; set; } = string.Empty;

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (string.IsNullOrEmpty(ConnectionString) || string.IsNullOrEmpty(ConnectionStringTemplate))
        {
            yield return new ValidationResult(
                $"{nameof(DatabaseSettings)}.{nameof(ConnectionString)} is not configured",
                new[] { nameof(ConnectionString) });
        }
    }
}