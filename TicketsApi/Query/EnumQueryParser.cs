
using System.Globalization;

namespace TicketsApi.Query;

public static class EnumQueryParser
{
    public static bool TryParse<TEnum>(
        string? value,
        out TEnum result
    ) where TEnum : struct, Enum
    {
        result = default;

        if (string.IsNullOrWhiteSpace(value))
        {
            return false;
        }

        var normalizedValue = value.Trim();

        if (int.TryParse(
            normalizedValue,
            NumberStyles.Integer,
            CultureInfo.InvariantCulture,
            out _))
        {
            return false;
        }

        return Enum.TryParse(
                   normalizedValue,
                   ignoreCase: true,
                   out result
               )
               && Enum.IsDefined(result);
    }
}
