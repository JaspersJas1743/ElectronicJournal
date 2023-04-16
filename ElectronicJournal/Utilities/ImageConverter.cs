using SharpVectors.Converters;
using System;
using System.Globalization;
using System.Windows.Data;

namespace ElectronicJournal.Utilities
{
	public class ImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
			=> new SvgViewbox() 
			{ 
				Source = new Uri(uriString: value.ToString(), uriKind: UriKind.Relative) 
			};

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
