using ElectronicJournal.ViewModels.Tools;
using System;
using System.ComponentModel;

namespace ElectronicJournal.Models
{
    public class ProfileModel : TrackedObject
    {
        public ProfileModel()
            => Array.ForEach(array: new TrackedObject[] { Email, Phone, Security }, action: to => to.PropertyChanged += OnModelPropertyChanged);

        private string _surname = default;
        private string _name = default;
        private string _patronymic = default;
        private bool _isMale = default;
        private bool _isFemale = default;
        private DateTime _birthday = default;
        private string _photo = default;
        public EmailBlock Email { get; set; } = new EmailBlock();
        public PhoneBlock Phone { get; set; } = new PhoneBlock();
        public SecurityBlock Security { get; set; } = new SecurityBlock();
        private string _folderForDownloads = default;
        private string[] _startedPages = default;
        private string _homePage = default;

        public class EmailBlock : TrackedObject
        {
            private string _email = default;

            public string Email
            {
                get => _email;
                set
                {
                    _email = value;
                    OnPropertyChanged(propertyName: nameof(Email));
                }
            }
        }

        public class PhoneBlock : TrackedObject
        {
            private string _phone = default;

            public string Phone
            {
                get => _phone;
                set
                {
                    _phone = value;
                    OnPropertyChanged(propertyName: nameof(Phone));
                }
            }
        }

        public class SecurityBlock : TrackedObject
        {
            private string _currentPassword = default;
            private string _newPassword = default;
            private string _confirmatedPassword = default;

            public string CurrentPassword
            {
                get => _currentPassword;
                set
                {
                    _currentPassword = value;
                    OnPropertyChanged(propertyName: nameof(CurrentPassword));
                }
            }

            public string NewPassword
            {
                get => _newPassword;
                set
                {
                    _newPassword = value;
                    OnPropertyChanged(propertyName: nameof(NewPassword));
                }
            }

            public string ConfirmatedPassword
            {
                get => _confirmatedPassword;
                set
                {
                    _confirmatedPassword = value;
                    OnPropertyChanged(propertyName: nameof(ConfirmatedPassword));
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(propertyName: nameof(Name));
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged(propertyName: nameof(Surname));
            }
        }

        public string Patronymic
        {
            get => _patronymic;
            set
            {
                _patronymic = value;
                OnPropertyChanged(propertyName: nameof(Patronymic));
            }
        }

        public bool IsMale
        {
            get => _isMale;
            set
            {
                _isMale = value;
                OnPropertyChanged(propertyName: nameof(IsMale));
            }
        }

        public bool IsFemale
        {
            get => _isFemale;
            set
            {
                _isFemale = value;
                OnPropertyChanged(propertyName: nameof(IsFemale));
            }
        }

        public DateTime Birthday
        {
            get => _birthday;
            set
            {
                _birthday = value;
                OnPropertyChanged(propertyName: nameof(Birthday));
            }
        }

        public string Photo
        {
            get => _photo;
            set
            {
                _photo = value;
                OnPropertyChanged(propertyName: nameof(Photo));
                OnPropertyChanged(propertyName: nameof(IsDefaultPhoto));
            }
        }

        public bool IsDefaultPhoto => _photo == null;

        public string FolderForDownloads
        {
            get => _folderForDownloads;
            set
            {
                _folderForDownloads = value;
                OnPropertyChanged(propertyName: nameof(FolderForDownloads));
            }
        }

        public string[] StartedPages
        {
            get => _startedPages;
            set
            {
                _startedPages = value;
                OnPropertyChanged(propertyName: nameof(StartedPages));
            }
        }

        public string HomePage
        {
            get => _homePage;
            set
            {
                _homePage = value;
                OnPropertyChanged(propertyName: nameof(HomePage));
            }
        }

        private void OnModelPropertyChanged(object sender, PropertyChangedEventArgs e)
            => OnPropertyChanged(propertyName: e.PropertyName);
    }
}
