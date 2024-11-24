using System;
using System.Globalization;
using System.Windows.Data;

namespace Expense_Tracker.Converters
{
    public class ThemeButtonTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isDarkTheme)
            {
                return isDarkTheme ? "Светлая тема" : "Темная тема";
            }
            return "Темная тема";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
