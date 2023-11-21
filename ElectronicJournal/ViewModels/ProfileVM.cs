using ElectronicJournal.Models;
using ElectronicJournal.Utilities.Config;
using ElectronicJournal.Utilities.Messages;
using ElectronicJournal.Utilities.PubSubEvents;
using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.ViewModels
{
    public class ProfileVM : VM
    {
        #region Fields
        private readonly IMessageProvider _message;
        private readonly IConfigProvider _config;
        private readonly IValidator<ProfileModel.SecurityBlock> _securityValidator;
        private readonly IValidator<ProfileModel.EmailBlock> _emailValidator;
        private readonly IValidator<ProfileModel.PhoneBlock> _phoneValidator;
        private readonly IEventAggregator _eventAggregator;

        private readonly Dictionary<string, string> _homePages = new Dictionary<string, string>
        {
            ["Профиль"] = "ProfileVM",
            ["Сообщения"] = "MessagesVM",
            ["Задания"] = "HomeworksVM",
            ["Оценки"] = "MarksVM",
            ["Расписание"] = "TimetableVM",
        };

        private ProfileModel _model;

        private readonly Lazy<Command> _loaded;
        private readonly Lazy<Command> _uploadPhoto;
        private readonly Lazy<Command> _emailChanging;
        private readonly Lazy<Command> _phoneChanging;
        private readonly Lazy<Command> _passwordChanging;
        private readonly Lazy<Command> _browseFolder;
        private readonly Lazy<Command> _logOut;
        #endregion Fields

        #region Constructor
        public ProfileVM(IMessageProvider message,
                         IConfigProvider config,
                         IValidator<ProfileModel.SecurityBlock> securityValidator,
                         IValidator<ProfileModel.EmailBlock> emailValidator,
                         IValidator<ProfileModel.PhoneBlock> phoneValidator,
                         IEventAggregator eventAggregator)
        {
            _message = message;
            _config = config; _securityValidator = securityValidator;
            _emailValidator = emailValidator;
            _phoneValidator = phoneValidator;
            _eventAggregator = eventAggregator;

            _model = new ProfileModel();
            _model.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(propertyName: e.PropertyName);

            _uploadPhoto = Command.CreateLazyCommand(action: async _ =>
            {
                string file = _message.OpenFile(filter: "Изображения (*.jpg, *.png, *.jpeg)|*.jpg; *.png; *.jpeg|Все файлы (*.*)|*.*");
                if (String.IsNullOrEmpty(file))
                    return;

                await User.UploadProfilePhoto(path: file);
                Photo = User.Photo;
                _eventAggregator.GetEvent<UserUploadPhotoEvent>().Publish(payload: new UserUploadPhotoEventArgs { NewPhoto = Photo });
            });

            _emailChanging = Command.CreateLazyCommand(
                action: async _ => await Change(func: async () => await User.ChangeEmail(newEmail: Email)),
                canExecute: _ => _emailValidator.Validate(instance: _model.Email).IsValid && Email != User.Email
            );

            _phoneChanging = Command.CreateLazyCommand(
                action: async _ => await Change(func: async () => await User.ChangePhone(newPhone: Phone)),
                canExecute: _ => _phoneValidator.Validate(instance: _model.Phone).IsValid && Phone != User.Phone
            );

            _passwordChanging = Command.CreateLazyCommand(action: async _ =>
            {
                if (await Change(func: async () => await User.ChangePassword(currentPassword: CurrentPassword, newPassword: NewPassword)))
                {
                    _model.Security.CurrentPassword = _model.Security.NewPassword = _model.Security.ConfirmatedPassword = String.Empty;
                    _config.SetMany(properties: new Dictionary<string, object> { ["Login"] = String.Empty, ["Password"] = String.Empty });
                }

            },
            canExecute: _ => _securityValidator.Validate(instance: _model.Security).IsValid);

            _browseFolder = Command.CreateLazyCommand(action: _ =>
            {
                string folder = _message.BrowseFolder();
                if (String.IsNullOrEmpty(folder))
                    return;

                FolderForDownloads = folder;
                _config.Set(propertyName: nameof(FolderForDownloads), value: FolderForDownloads);
            });

            _logOut = Command.CreateLazyCommand(action: _ =>
            {
                User.LogOut();
                _config.SetMany(properties: new Dictionary<string, object> { ["Login"] = String.Empty, ["Password"] = String.Empty });
                _eventAggregator.GetEvent<ChangeMainWindowContentEvent>().Publish(payload: new ChangeMainWindowContentEventArgs { NewVM = Program.AppHost.Services.GetService<AuthorizationVM>() });
            });

            _loaded = Command.CreateLazyCommand(action: async _ =>
            {
                Surname = User.Surname;
                Name = User.Name;
                Patronymic = User.Patronymic;
                IsMale = User.Gender.Equals("Мужской") ? true : false;
                IsFemale = !IsMale;
                Birthday = User.Birthday;
                Photo = User.Photo;
                Email = User.Email;
                Phone = User.Phone;
                FolderForDownloads = _config.Get<string>(propertyName: nameof(FolderForDownloads));
                StartedPages = _homePages.Keys.ToArray();
                HomePage = _homePages.First(predicate: p => p.Value == _config.Get<string>(propertyName: nameof(HomePage))).Key;
            });
        }

        private async Task<bool> Change(Func<Task<User.ChangeResponse>> func)
        {
            User.ChangeResponse result = await func();
            if (result.IsSuccess)
                _message.ShowInformation(text: result.Message);
            else _message.ShowError(text: result.Message);
            return result.IsSuccess;
        }
        #endregion Constructor

        #region Properties
        public string Name
        {
            get => _model.Name;
            set => _model.Name = value;
        }

        public string Surname
        {
            get => _model.Surname;
            set => _model.Surname = value;
        }

        public string Patronymic
        {
            get => _model.Patronymic;
            set => _model.Patronymic = value;
        }

        public bool IsMale
        {
            get => _model.IsMale;
            set => _model.IsMale = value;
        }

        public bool IsFemale
        {
            get => _model.IsFemale;
            set => _model.IsFemale = value;
        }

        public DateTime Birthday
        {
            get => _model.Birthday;
            set => _model.Birthday = value;
        }

        public string Photo
        {
            get => _model.Photo;
            set => _model.Photo = value;
        }

        public bool IsDefaultPhoto => _model.IsDefaultPhoto;

        public string Email
        {
            get => _model.Email.Email;
            set => _model.Email.Email = value;
        }

        public string Phone
        {
            get => _model.Phone.Phone;
            set => _model.Phone.Phone = value;
        }

        public string CurrentPassword
        {
            get => _model.Security.CurrentPassword;
            set => _model.Security.CurrentPassword = value;
        }

        public string NewPassword
        {
            get => _model.Security.NewPassword;
            set => _model.Security.NewPassword = value;
        }

        public string ConfirmatedPassword
        {
            get => _model.Security.ConfirmatedPassword;
            set => _model.Security.ConfirmatedPassword = value;
        }

        public string FolderForDownloads
        {
            get => _model.FolderForDownloads;
            set => _model.FolderForDownloads = value;
        }

        public string[] StartedPages
        {
            get => _model.StartedPages;
            set => _model.StartedPages = value;
        }

        public string HomePage
        {
            get => _model.HomePage;
            set
            {
                _model.HomePage = value;
                _config.Set(propertyName: nameof(HomePage), value: _homePages[key: value]);
            }
        }

        public User User { get; set; }

        public Command Loaded => _loaded.Value;
        public Command UploadPhoto => _uploadPhoto.Value;
        public Command EmailChanging => _emailChanging.Value;
        public Command PhoneChanging => _phoneChanging.Value;
        public Command PasswordChanging => _passwordChanging.Value;
        public Command BrowseFolder => _browseFolder.Value;
        public Command LogOut => _logOut.Value;
        #endregion Properties
    }
}
