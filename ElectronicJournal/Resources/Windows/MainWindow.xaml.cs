using System.Windows;
using System.Windows.Input;

namespace ElectronicJournal.Resources.Windows
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void DraggingWindow(object sender, MouseButtonEventArgs e)
		{
			if (Mouse.LeftButton.Equals(obj: MouseButtonState.Pressed))
				DragMove();
		}
	}
}
