using ElectronicJournal.Utilities;
using System.Windows;

namespace ElectronicJournal
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Navigation.Frame = MainFrame;
        }
    }
}
