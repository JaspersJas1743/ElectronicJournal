using ElectronicJournal.Utilities;
using ElectronicJournal.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicJournal.Resources.UserControls
{
	public partial class SvgWithChangeOfTheme : UserControl
	{
		#region Fields
		private SvgWithChangeOfThemeVM _vm = new SvgWithChangeOfThemeVM();

		public static readonly DependencyProperty LightThemeSourceProperty = DependencyProperty.Register(
			name: "LightThemeSource", propertyType: typeof(String), ownerType: typeof(SvgWithChangeOfTheme)
		);

		public static readonly DependencyProperty DarkThemeSourceProperty = DependencyProperty.Register(
			name: "DarkThemeSource", propertyType: typeof(String), ownerType: typeof(SvgWithChangeOfTheme)
		);
		#endregion Fields

		#region Constructors
		public SvgWithChangeOfTheme()
		{
			InitializeComponent();
			Theme.ThemeChanged += OnThemeChanged;
			this.DataContext = _vm;
		}
		#endregion Constructors

		#region Properties
		public string LightThemeSource
		{
			get => (string)GetValue(dp: LightThemeSourceProperty);
			set => SetValue(dp: LightThemeSourceProperty, value: value);
		}

		public string DarkThemeSource
		{
			get => (string)GetValue(dp: DarkThemeSourceProperty);
			set => SetValue(dp: DarkThemeSourceProperty, value: value);
		}
		#endregion Properties

		#region Methods
		private void OnThemeChanged(object sender, ThemeChangedEventArgs e)
		{
			(_vm.LightThemeSourceVisiblity, _vm.DarkThemeSourceVisiblity) = (_vm.DarkThemeSourceVisiblity, _vm.LightThemeSourceVisiblity);
		}
		#endregion Methods
	}
}
