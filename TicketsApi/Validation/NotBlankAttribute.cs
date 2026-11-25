
using System.ComponentModel.DataAnnotations;

namespace TicketsApi.Validation;

[AttributeUsage(AttributeTargets.Property)]
public sealed class NotBlankAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return value is string text &&
               !string.IsNullOrWhiteSpace(text);
    }
}
