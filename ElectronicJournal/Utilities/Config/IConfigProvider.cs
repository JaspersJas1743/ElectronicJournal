namespace ElectronicJournal.Utilities.Config
{
    public interface IConfigProvider
    {
        T Get<T>(string propertyName);

        void Set(string propertyName, object value);
    }
}
