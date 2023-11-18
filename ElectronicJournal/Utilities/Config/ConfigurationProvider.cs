using System;
using System.Collections.Generic;
using System.Reflection;

namespace ElectronicJournal.Utilities.Config
{
    public class ConfigurationProvider : IConfigProvider
    {
        public T Get<T>(string propertyName)
            => (T)GetProperty(propertyName: propertyName).GetValue(obj: Properties.Settings.Default);

        public void Set(string propertyName, object value)
        {
            GetProperty(propertyName: propertyName).SetValue(obj: Properties.Settings.Default, value: value);
            Properties.Settings.Default.Save();
        }

        public void SetMany(Dictionary<string, object> properties)
        {
            foreach (var property in properties)
                GetProperty(propertyName: property.Key).SetValue(obj: Properties.Settings.Default, value: property.Value);

            Properties.Settings.Default.Save();
        }

        private PropertyInfo GetProperty(string propertyName)
            => Properties.Settings.Default.GetType().GetProperty(name: propertyName) ?? throw new ArgumentException(message: "Некорректное имя свойства", paramName: nameof(propertyName));
    }
}
