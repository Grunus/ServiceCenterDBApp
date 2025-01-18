using Library.Miscellaneous;
using System.Windows.Data;

namespace WPFApp.Converters
{
    public class EnumToString : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return (value as Enum).GetEnumDescription();
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
