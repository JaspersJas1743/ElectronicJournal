using System;
using System.Configuration;

namespace ElectronicJournal.Utilities
{
	public static class ConfigProvider
	{
		private static Configuration _config = ConfigurationManager.OpenExeConfiguration(userLevel: ConfigurationUserLevel.None);

		public static T Get<T>(string proprtyName)
			=> (T)Convert.ChangeType(value: _config.AppSettings.Settings[key: proprtyName].Value, conversionType: typeof(T));

		public static void Set(string propertyName, object value)
		{
			_config.AppSettings.Settings[propertyName].Value = value.ToString();
			_config.Save();
			ConfigurationManager.RefreshSection(sectionName: _config.AppSettings.SectionInformation.Name);
		}
	}
}
