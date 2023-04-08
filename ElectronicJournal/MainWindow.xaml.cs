using ElectronicJournal.Utilities;
using ElectronicJournal.Views;
using System.Windows;

namespace ElectronicJournal
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			Navigation.Frame = MainFrame;
			Navigation.Navigate(page: new Authorization());
		}
	}
}
