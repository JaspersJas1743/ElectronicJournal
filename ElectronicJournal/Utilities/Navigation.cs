using System.Windows.Controls;

namespace ElectronicJournal.Utilities
{
	public static class Navigation
	{
		public static Frame Frame { get; set; }

		public static void Navigate(object page)
			=> Frame.Navigate(content: page);
	}
}
