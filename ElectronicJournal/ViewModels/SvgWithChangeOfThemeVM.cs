using ElectronicJournal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronicJournal.ViewModels
{
	public class SvgWithChangeOfThemeVM: TrackedObject
	{
		#region Fields
		private Visibility _lightThemeSourceVisiblity;
		private Visibility _darkThemeSourceVisiblity;
		#endregion Fields

		#region Constructors
		public SvgWithChangeOfThemeVM()
		{
			_lightThemeSourceVisiblity = Theme.CurrentTheme == Theme.Type.Light? Visibility.Visible : Visibility.Collapsed;
			_darkThemeSourceVisiblity = Theme.CurrentTheme == Theme.Type.Dark ? Visibility.Visible : Visibility.Collapsed; 
		}
		#endregion Constructors

		#region Properties
		public Visibility LightThemeSourceVisiblity
		{
			get => _lightThemeSourceVisiblity;
			set
			{
				_lightThemeSourceVisiblity = value;
				OnPropertyChanged(nameof(LightThemeSourceVisiblity));
			}
		}

		public Visibility DarkThemeSourceVisiblity
		{
			get => _darkThemeSourceVisiblity;
			set
			{
				_darkThemeSourceVisiblity = value;
				OnPropertyChanged(nameof(DarkThemeSourceVisiblity));
			}
		}
		#endregion Properties
	}
}
