using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI;

namespace ElectronicJournal.Models
{
    public class MenuModel : TrackedObject
    {
        private string _greeting = default;
        private string _fullName = default;
        private string _photo = default;

        public string Photo
        {
            get => _photo;
            set
            {
                _photo = value;
                OnPropertyChanged(propertyName: nameof(Photo));
            }
        }

        public string Greeting
        {
            get => _greeting;
            set
            {
                _greeting = value;
                OnPropertyChanged(propertyName: nameof(Greeting));
            }
        }

        public string FullName
        {
            get => _fullName;
            set
            {
                _fullName = value;
                OnPropertyChanged(propertyName: nameof(FullName));
            }
        }
    }
}
