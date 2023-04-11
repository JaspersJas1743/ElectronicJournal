using System;
using System.Collections.Generic;
using System.Windows;
using System.Configuration;

namespace ElectronicJournal.Utilities
{
    public static class Theme
    {
        private static Dictionary<string, string> _helper = new Dictionary<string, string>()
        {
            ["Light"] = "Dark",
            ["Dark"] = "Light"
        };

        private static Configuration _config;

        static Theme()
        {
            _config = ConfigurationManager.OpenExeConfiguration(userLevel: ConfigurationUserLevel.None);
        }

        public static string CurrentTheme
        {
            get => _config.AppSettings.Settings["Theme"].Value;
            set
            {
                _config.AppSettings.Settings["Theme"].Value = value;
                _config.Save();
                ConfigurationManager.RefreshSection(sectionName: _config.AppSettings.SectionInformation.Name);
            }
        }

        public static void Init()
            => Load();

        public static void Change()
        {
            CurrentTheme = _helper[CurrentTheme];
            Load();
        }

        private static void Load()
        {
            Uri uri = new Uri(uriString: $"/Resources/Themes/{CurrentTheme}Colors.xaml", uriKind: UriKind.Relative);
            ResourceDictionary dictionary = (ResourceDictionary)Application.LoadComponent(resourceLocator: uri);
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(item: dictionary);
        }
    }
}
