using System;
using System.Globalization;
using System.Windows.Data;

namespace ElectronicJournal.Views.Tools
{
    public class DayToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                string result = date.ToString("dddd");
                return char.ToUpper(c: result[0]) + result.Substring(startIndex: 1);
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
