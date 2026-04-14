using System.Globalization;
using System.Windows.Data;

namespace MyCoolSDR.Converters;

/// <summary>
/// Converts bandwidth values from Hertz to Kilohertz for display in the UI.
/// </summary>
public class HzToKhzConverter : IValueConverter
{
    private const ulong DivisorKhz = 1_000;
    private const string DisplayFormat = "F3";

    /// <summary>
    /// Converts a Hertz value to formatted Kilohertz string.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ulong hz)
        {
            return (hz / DivisorKhz).ToString(DisplayFormat, culture);
        }
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
