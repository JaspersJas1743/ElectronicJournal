using ElectronicJournal.Utilities.Navigation;
using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronicJournal.ViewModels
{
    public class MenuVM : ContentPresenter
    {
        #region Fields
        private readonly INavigationProvider _navigationProvider;

        private readonly Lazy<Command> _profile;
        private readonly Lazy<Command> _messages;
        private readonly Lazy<Command> _homeworks;
        private readonly Lazy<Command> _marks;
        private readonly Lazy<Command> _timetable;
        private readonly Lazy<Command> _loaded;

        private string _greeting;
        private string _fullName;
        private string _photo;
        #endregion Fields

        #region Constructor
        public MenuVM(INavigationProvider navigationProvider)
        {
            _navigationProvider = navigationProvider;

            _profile = Command.CreateLazyCommand(action: _ => _navigationProvider.MoveTo<MenuVM, ProfileVM>());
            _messages = Command.CreateLazyCommand(action: _ => _navigationProvider.MoveTo<MenuVM, MessagesVM>());
            _homeworks = Command.CreateLazyCommand(action: _ => _navigationProvider.MoveTo<MenuVM, HomeworksVM>());
            _marks = Command.CreateLazyCommand(action: _ => _navigationProvider.MoveTo<MenuVM, MarksVM>());
            _timetable = Command.CreateLazyCommand(action: _ => _navigationProvider.MoveTo<MenuVM, TimetableVM>());
            _loaded = Command.CreateLazyCommand(action: async _ =>
            {
                Task downloadPhotoTask = User.DownloadProfilePhoto();
                Greeting = new string[] { "Доброй ночи", "Доброе утро", "Добрый день", "Добрый вечер" }[DateTime.Now.Hour / 6] + ',';
                FullName = $"{User.Name} {User.Patronymic}";
                await downloadPhotoTask;
                Photo = User.Photo;
            });
        }
        #endregion Construtor

        #region Properties
        public User User { get; set; }

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

        public Command Profile => _profile.Value;
        public Command Messages => _messages.Value;
        public Command Homeworks => _homeworks.Value;
        public Command Marks => _marks.Value;
        public Command Timetable => _timetable.Value;
        public Command Loaded => _loaded.Value;
        #endregion Properties

        #region Methods
        #endregion Methods
    }
}