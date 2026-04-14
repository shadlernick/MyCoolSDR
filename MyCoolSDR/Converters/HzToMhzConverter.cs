using System.Globalization;
using System.Windows.Data;

namespace MyCoolSDR.Converters;

/// <summary>
/// Converts frequency values from Hertz to Megahertz for display in the UI.
/// </summary>
public class HzToMhzConverter : IValueConverter
{
    private const ulong DivisorMhz = 1_000_000;
    private const string DisplayFormat = "F3";

    /// <summary>
    /// Converts a Hertz value to formatted Megahertz string.
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ulong hz)
            return (hz / (double)DivisorMhz).ToString(DisplayFormat, culture);

        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}