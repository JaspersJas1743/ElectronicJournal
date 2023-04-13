using System.Windows;

namespace ElectronicJournal.Utilities
{
	public class Notification
	{
		public static MessageBoxResult Show(string message)
		{
			return MessageBox.Show(messageBoxText: message,
				caption: "Уведомление",
				button: MessageBoxButton.YesNo,
				icon: MessageBoxImage.Question);
		}
	}
}
