using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace ElectronicJournal.Views.Tools
{
    public class MarkColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string mark)
            {
                string result;
                if (mark == "Н")
                    return Application.Current.FindResource(resourceKey: "Skip") as SolidColorBrush;

                double note = Double.Parse(s: mark);
                result = NewMethod(note);

                return Application.Current.FindResource(resourceKey: result) as SolidColorBrush;
            } else if (value is double m)
                return Application.Current.FindResource(resourceKey: NewMethod(note: m)) as SolidColorBrush;
            return Binding.DoNothing;
        }

        private static string NewMethod(double note)
        {
            string result;
            if (note >= 4.5 && note < 5)
                result = "Five";
            else if (note >= 3.5)
                result = "Four";
            else if (note >= 2.5)
                result = "Three";
            else if (note == 0)
                return "Skip";
            else
                result = "Two";
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
