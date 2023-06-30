using System;
using System.Configuration;

namespace ElectronicJournal.Utilities.Config
{
	public class ConfigurationProvider : IConfigProvider
	{
		private Configuration _config = ConfigurationManager.OpenExeConfiguration(userLevel: ConfigurationUserLevel.None);

		public T Get<T>(string propertyName)
			=> (T)Convert.ChangeType(value: _config.AppSettings.Settings[key: propertyName].Value, conversionType: typeof(T));

		public void Set(string propertyName, object value)
		{
			_config.AppSettings.Settings[propertyName].Value = value.ToString();
			_config.Save();
			ConfigurationManager.RefreshSection(sectionName: _config.AppSettings.SectionInformation.Name);
		}
	}
}
