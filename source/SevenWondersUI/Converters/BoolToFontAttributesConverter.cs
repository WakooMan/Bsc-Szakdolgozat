using System.Globalization;

namespace SevenWondersUI.Converters
{
    public class BoolToFontAttributesConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is bool b && b)
                return FontAttributes.Bold;
            return FontAttributes.None;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is FontAttributes attributes && attributes == FontAttributes.Bold;
        }
    }
}
