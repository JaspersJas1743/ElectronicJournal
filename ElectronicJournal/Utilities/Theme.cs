using System;
using System.Windows;

namespace ElectronicJournal.Utilities
{
	public static class Theme
	{
		#region Enums
		public enum Type
		{
			Light,
			Dark
		}
		#endregion Enums

		#region Properties
		public static Type CurrentTheme
		{
			get => Theme.Parse(themeName: ConfigProvider.Get<String>(proprtyName: "Theme"));
			set => ConfigProvider.Set(propertyName: "Theme", value: value.ToString());
		}
		#endregion Properties

		#region Delegate
		public delegate void ThemeChangedEventHandler(object sender, ThemeChangedEventArgs e);
		#endregion Delegate

		#region Events
		public static event ThemeChangedEventHandler ThemeChanged;
		#endregion Events

		#region Methods
		public static Type Parse(string themeName)
			=> (Type)Enum.Parse(enumType: typeof(Type), value: themeName);

		public static void Init()
			=> Load();

		public static void Change(Type newTheme)
		{
			Type oldTheme = CurrentTheme;
			CurrentTheme = newTheme;
			Load();
			ThemeChanged?.Invoke(sender: null, e: new ThemeChangedEventArgs(oldTheme: oldTheme, newTheme: newTheme));
		}

		private static void Load()
		{
			Uri uri = new Uri(uriString: $"/Resources/Styles/{CurrentTheme}Colors.xaml", uriKind: UriKind.Relative);
			ResourceDictionary dictionary = (ResourceDictionary)Application.LoadComponent(resourceLocator: uri);
			Application.Current.Resources.Clear();
			Application.Current.Resources.MergedDictionaries.Add(item: dictionary);
		}
		#endregion Methods
	}
}
