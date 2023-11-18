using ElectronicJournal.Models;
using ElectronicJournal.Utilities.Config;
using ElectronicJournal.Utilities.PubSubEvents;
using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using ElectronicJournalAPI.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace ElectronicJournal.ViewModels
{
    public class MenuVM : ContentPresenter
    {
        #region Fields
        private readonly IConfigProvider _config;
        private readonly IEventAggregator _eventAggregator;

        private MenuModel _model;

        private readonly Lazy<Command> _profile;
        private readonly Lazy<Command> _messages;
        private readonly Lazy<Command> _homeworks;
        private readonly Lazy<Command> _marks;
        private readonly Lazy<Command> _timetable;
        private readonly Lazy<Command> _loaded;
        private readonly Lazy<Command> _information;
        #endregion Fields

        #region Constructor
        public MenuVM(IConfigProvider config, IEventAggregator eventAggregator)
        {
            _config = config;
            _eventAggregator = eventAggregator;

            _model = new MenuModel();
            _model.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(propertyName: e.PropertyName);

            _eventAggregator.GetEvent<UserUploadPhotoEvent>().Subscribe(action: UploadPhoto);
            _eventAggregator.GetEvent<ChangeMessageToCreationMessageEvent>().Subscribe(action: ChangeMessageToCreationMessage);
            _eventAggregator.GetEvent<CloseMessageCreationAfterSendingEvent>().Subscribe(action: CloseMessageCreationAfterSending);

            _profile = Command.CreateLazyCommand(action: _ => MoveTo<ProfileVM>());
            _messages = Command.CreateLazyCommand(action: _ => MoveTo<MessagesVM>());
            _homeworks = Command.CreateLazyCommand(action: _ => MoveTo<HomeworksVM>());
            _marks = Command.CreateLazyCommand(action: _ => MoveTo<MarksVM>());
            _timetable = Command.CreateLazyCommand(action: _ => MoveTo<TimetableVM>());
            _information = Command.CreateLazyCommand(action: _ => MoveToAboutVM());
            _loaded = Command.CreateLazyCommand(action: async _ =>
            {
                Task downloadPhoto = User.DownloadProfilePhoto();
                Greeting = new string[] { "Доброй ночи", "Доброе утро", "Добрый день", "Добрый вечер" }[DateTime.Now.Hour / 6] + ',';
                FullName = $"{User.Name} {User.Patronymic}";
                await downloadPhoto;
                Photo = User.Photo;

                MoveTo(to: Type.GetType(typeName: "ElectronicJournal.ViewModels." + HomePage));
            });
        }
        #endregion Construtor

        ~MenuVM()
        {
            _eventAggregator.GetEvent<UserUploadPhotoEvent>().Unsubscribe(subscriber: UploadPhoto);
        }

        #region Properties
        public string Photo
        {
            get => _model.Photo;
            set => _model.Photo = value;
        }

        public string Greeting
        {
            get => _model.Greeting;
            set => _model.Greeting = value;
        }

        public string FullName
        {
            get => _model.FullName;
            set => _model.FullName = value;
        }

        public string HomePage => _config.Get<string>(propertyName: nameof(HomePage));

        public Command Profile => _profile.Value;
        public Command Messages => _messages.Value;
        public Command Homeworks => _homeworks.Value;
        public Command Marks => _marks.Value;
        public Command Timetable => _timetable.Value;
        public Command Info => _information.Value;
        public Command Loaded => _loaded.Value;

        public User User { get; set; }
        #endregion Properties

        #region Methods
        private void MoveTo<T>() where T : VM
            => MoveTo(to: typeof(T));

        private void MoveToAboutVM()
        {
            AboutVM vm = Program.AppHost.Services.GetRequiredService<AboutVM>();
            Content = vm;
        }

        private void MoveTo(Type to)
        {
            if (Content?.GetType() == to)
                return;

            VM vm = Program.AppHost.Services.GetRequiredService(serviceType: to) as VM;
            to.GetProperty(name: "User").SetValue(obj: vm, value: User);
            Content = vm;
        }

        private void UploadPhoto(UserUploadPhotoEventArgs e)
            => Photo = e.NewPhoto;

        private async void ChangeMessageToCreationMessage(ChangeMessageToCreationMessageEventArgs e)
        {
            MessageCreationVM vm = Program.AppHost.Services.GetRequiredService<MessageCreationVM>();
            vm.User = e.User;
            await SetMessageInformation(e, vm);
            Content = vm;
        }

        private async Task SetMessageInformation(ChangeMessageToCreationMessageEventArgs e, MessageCreationVM vm)
        {
            if (e.Message == null)
                return;

            vm.Text = e.Message.Text;
            vm.SelectedUser = new User.MessageReceiversResponse { Id = e.Message.SenderId, DisplayedName = e.Message.Sender };
            vm.Filter = vm.SelectedUser.DisplayedName;

            if (e.Message.Attachment == null)
                return;

            string tempFolder = new TempDirectory().Path;
            await e.Message.Attachment.Download(folder: tempFolder);
            vm.AddAttachment(Path.Combine(path1: tempFolder, path2: e.Message.Attachment.FileName));
        }

        private void CloseMessageCreationAfterSending(CloseMessageCreationAfterSendingEventArgs e)
        {
            MessagesVM vm = Program.AppHost.Services.GetRequiredService<MessagesVM>();
            vm.User = e.User;
            vm.SelectedIndex = 1;
            Content = vm;
        }
        #endregion Methods
    }
}