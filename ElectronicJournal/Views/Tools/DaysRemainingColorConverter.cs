using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ElectronicJournal.Views.Tools
{
    public class DaysRemainingColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int countRemainDays)
            {
                if (countRemainDays <= 1)
                    return Application.Current.FindResource(resourceKey: "LittleTimeBeforeDelivery") as SolidColorBrush;

                if (new int[] { 2, 3, 4 }.Contains(value: countRemainDays))
                    return Application.Current.FindResource(resourceKey: "EnoughTimeBeforeDelivery") as SolidColorBrush;

                return Application.Current.FindResource(resourceKey: "ManyTimeBeforeDelivery") as SolidColorBrush;
            }
            throw new ArgumentException(message: "Incorrect count of remain days", paramName: nameof(value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
