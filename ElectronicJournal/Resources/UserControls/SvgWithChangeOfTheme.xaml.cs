using ElectronicJournal.Utilities;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicJournal.Resources.UserControls
{
	public partial class SvgWithChangeOfTheme : UserControl
	{
		#region Fields
		public static readonly DependencyProperty SvgFileNameProperty = DependencyProperty.Register(
			name: "SvgFileName", propertyType: typeof(String), ownerType: typeof(SvgWithChangeOfTheme)
		);
		#endregion Fields

		#region Constructors
		public SvgWithChangeOfTheme()
		{
			InitializeComponent();
			Theme.ThemeChanged += OnThemeChanging;
		}
		#endregion Constructors

		#region Properties
		public string SvgFileName
		{
			get => (string)GetValue(dp: SvgFileNameProperty);
			set => SetValue(dp: SvgFileNameProperty, value: value);
		}

		#endregion Properties

		#region Methods
		private void OnThemeChanging(object sender, ThemeChangedEventArgs e)
		{
			Uri uri = new Uri(uriString: SvgFileName.Replace(oldValue: e.OldTheme.ToString(), newValue: e.NewTheme.ToString()));
			SvgPresenter.Unload();
			SvgPresenter.Load(uriSource: uri);
		}
		#endregion Methods
	}
}
