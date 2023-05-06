namespace ElectronicJournal.Utilities
{
	public class Error
	{
		public Error(string message, string propertyName)
		{
			Message = message;
			PropertyName = propertyName;
		}

		public string Message { get; }
		public string PropertyName { get; }
	}
}
