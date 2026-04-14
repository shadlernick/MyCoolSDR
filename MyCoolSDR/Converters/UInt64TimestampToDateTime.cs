using System.Globalization;
using System.Windows.Data;

namespace MyCoolSDR.Converters;

/// <summary>
/// Converts timestamp values from ulong to DateTime for display in the UI.
/// </summary>
public class UInt64TimestampToDateTime : IValueConverter
{
    /// <summary>
    /// Converts a ulong value to formatted DateTime string.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ulong time)
            return DateTimeOffset.FromUnixTimeSeconds((long)time).UtcDateTime;

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
