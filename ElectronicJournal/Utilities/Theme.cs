using System;
using System.Configuration;
using System.Windows;

namespace ElectronicJournal.Utilities
{
	public static class Theme
	{
		private static Configuration _config;

		static Theme()
		{
			_config = ConfigurationManager.OpenExeConfiguration(userLevel: ConfigurationUserLevel.None);
		}

		public enum Type
		{
			Light,
			Dark
		}

		public static Type CurrentTheme
		{
			get => (Type)Enum.Parse(enumType: typeof(Type), value: _config.AppSettings.Settings["Theme"].Value);
			set
			{
				_config.AppSettings.Settings["Theme"].Value = value.ToString();
				_config.Save();
				ConfigurationManager.RefreshSection(sectionName: _config.AppSettings.SectionInformation.Name);
			}
		}

		#region Methods
		public static void Init()
			=> Load();

		public static void Change(Type newTheme)
		{
			CurrentTheme = newTheme;
			Load();
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
