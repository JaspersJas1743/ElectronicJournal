using ElectronicJournal.Models;
using System;
using System.Timers;
using System.Windows;

namespace ElectronicJournal
{
	public partial class MainWindow : Window
	{
		private TimeModel _time = new TimeModel();

		public MainWindow()
		{
			InitializeComponent();
			Time.DataContext = _time;
			Timer timer = new Timer(interval: 1000);
			timer.Elapsed += (sender, e) => _time.Time = DateTime.Now;
			timer.Start();
		}

		private void OnBackButtonClick(object sender, RoutedEventArgs e)
		{
		}

		private void OnWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
			=> e.Cancel = !MessageBox.Show(
					messageBoxText: "Вы уверены, что хотите закрыть приложение?",
					caption: "Закрыть",
					button: MessageBoxButton.YesNo,
					icon: MessageBoxImage.Warning
				).Equals(obj: MessageBoxResult.Yes);
	}
}
