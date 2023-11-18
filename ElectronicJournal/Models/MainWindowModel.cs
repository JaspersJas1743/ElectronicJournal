using ElectronicJournal.ViewModels.Tools;
using System.Windows;

namespace ElectronicJournal.Models
{
    public class MainWindowModel : TrackedObject
    {
        private Visibility _collapseVisibility = default;
        private Visibility _expandVisibility = default;
        private double _width = default;
        private double _height = default;
        private bool _isOn = default;
        private WindowState _windowState = default;

        public Visibility ExpandVisibility
        {
            get => _expandVisibility;
            set
            {
                _expandVisibility = value;
                OnPropertyChanged(propertyName: nameof(ExpandVisibility));
            }
        }

        public Visibility CollapseVisibility
        {
            get => _collapseVisibility;
            set
            {
                _collapseVisibility = value;
                OnPropertyChanged(propertyName: nameof(CollapseVisibility));
            }
        }
        public bool IsOn
        {
            get => _isOn;
            set
            {
                _isOn = value;
                OnPropertyChanged(propertyName: nameof(IsOn));
            }
        }

        public double Width
        {
            get => _width;
            set
            {
                _width = value;
                OnPropertyChanged(propertyName: nameof(Width));
            }
        }

        public double Height
        {
            get => _height;
            set
            {
                _height = value;
                OnPropertyChanged(propertyName: nameof(Height));
            }
        }

        public WindowState WindowState 
        {
            get => _windowState;
            set
            {
                _windowState = value;
                OnPropertyChanged(propertyName: nameof(WindowState));
            }
        }
    }
}
