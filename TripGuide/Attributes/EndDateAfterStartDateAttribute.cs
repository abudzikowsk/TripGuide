using System.ComponentModel.DataAnnotations;

namespace TripGuide.Attributes;

public class EndDateAfterStartDateAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var startDate = validationContext.ObjectInstance.GetType().GetProperty("StartDate")?
            .GetValue(validationContext.ObjectInstance) as DateTime?;

        var endDate = value as DateTime?;

        if (startDate is null || endDate is null)
        {
            return new ValidationResult("StartDate and EndDate need to have a value.");
        }

        if (startDate > endDate)
        {
            return new ValidationResult("StartDate need to be before EndDate.");
        }

        return ValidationResult.Success;
    }
}