using ElectronicJournal.Utilities.Config;
using System;
using System.Windows;

namespace ElectronicJournal.Utilities
{
    public static class Theme
    {
        #region Fields
        private static readonly IConfigProvider _config;
        #endregion Fields

        #region Constructors
        static Theme()
        {
            _config = new ConfigurationProvider();
        }
        #endregion Constructors

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
            get => Theme.Parse(themeName: _config.Get<String>(propertyName: nameof(Theme)));
            set => _config.Set(propertyName: nameof(Theme), value: value.ToString());
        }
        #endregion Properties

        #region Methods
        public static Type Parse(string themeName)
            => (Type)Enum.Parse(enumType: typeof(Type), value: themeName);

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
