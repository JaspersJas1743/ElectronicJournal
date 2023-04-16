using System;

namespace ElectronicJournal.Utilities
{
	public class ThemeChangedEventArgs: EventArgs
	{
		public ThemeChangedEventArgs(Theme.Type oldTheme, Theme.Type newTheme)
		{
			OldTheme = oldTheme;
			NewTheme = newTheme;
		}

		public Theme.Type OldTheme { get; }

		public Theme.Type NewTheme { get; }
	}
}