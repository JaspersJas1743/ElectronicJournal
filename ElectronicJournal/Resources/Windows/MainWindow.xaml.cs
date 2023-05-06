using ElectronicJournal.Utilities;
using System;
using System.Windows;

namespace ElectronicJournal.Resources.Windows
{
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			this.Width = ConfigProvider.Get<Double>(proprtyName: "Width");
			this.Height = ConfigProvider.Get<Double>(proprtyName: "Height");
		}

		private void ThisWindow_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			ConfigProvider.Set(propertyName: "Width", value: e.NewSize.Width);
			ConfigProvider.Set(propertyName: "Height", value: e.NewSize.Height);
		}
	}
}
