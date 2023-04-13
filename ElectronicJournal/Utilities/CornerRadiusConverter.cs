using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ElectronicJournal.Utilities
{
	public class CornerRadiusConverter : IValueConverter
	{
		private double CornerRadius => 5000 / Application.Current.MainWindow.Height;

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> CornerRadius;

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> CornerRadius;
	}
}
