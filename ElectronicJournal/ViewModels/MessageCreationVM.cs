using ElectronicJournal.Models;
using ElectronicJournal.Utilities.Messages;
using ElectronicJournal.Utilities.PubSubEvents;
using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using ElectronicJournalAPI.Utilities;
using FluentValidation;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.ViewModels
{
    public class MessageCreationVM : VM
    {
        private IMessageProvider _message;
        private IEventAggregator _eventAggregator;
        private IValidator<MessageCreationModel> _validator;

        private MessageCreationModel _model;

        private readonly Lazy<Command> _addAttachments;
        private readonly Lazy<Command> _sendMessage;

        public MessageCreationVM(IMessageProvider message, IEventAggregator eventAggregator, IValidator<MessageCreationModel> validator)
        {
            _message = message;
            _eventAggregator = eventAggregator;
            _validator = validator;

            _model = new MessageCreationModel();
            _model.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(propertyName: e.PropertyName);
            ChangeDisplayedAttahmentCount();
            AttachmentsToolTip = GetToolTip();

            _addAttachments = Command.CreateLazyCommand(action: _ =>
            {
                string[] paths = _message.OpenManyFile();
                foreach (string path in paths)
                    AddAttachment(path: path);
            });

            _sendMessage = Command.CreateLazyCommand(action: async _ =>
            {
                string msg = await new Message
                {
                    ReceiverId = SelectedUser.Id,
                    Text = Text,
                    Attachment = Attachments.Count == 0 ? null : new Attachment(files: Attachments)
                }.Send();
                _message.ShowMessage(text: msg);
                CloseMessageCreationAfterSendingEventArgs e = new CloseMessageCreationAfterSendingEventArgs { User = User };
                _eventAggregator.GetEvent<CloseMessageCreationAfterSendingEvent>().Publish(payload: e);
            }, canExecute: _ => _validator.Validate(instance: _model).IsValid);
        }

        public User User { get; set; }

        public IEnumerable<User.MessageReceiversResponse> Users
        {
            get => _model.Users;
            set => _model.Users = value;
        }

        public string Filter
        {
            get => _model.Filter;
            set
            {
                _model.Filter = value;
                UsersFilter();
            }
        }

        public User.MessageReceiversResponse SelectedUser
        {
            get => _model.SelectedUser;
            set => _model.SelectedUser = value;
        }

        public List<string> Attachments => _model.Attachments;

        public string AttachmentsToolTip
        {
            get => _model.AttachmentsToolTip;
            set => _model.AttachmentsToolTip = value;
        }

        public string Text
        {
            get => _model.Text;
            set => _model.Text = value;
        }

        public string DisplayedAttachmentsCount
        {
            get => _model.DisplayedAttachmentsCount;
            set => _model.DisplayedAttachmentsCount = value;
        }

        public void AddAttachment(string path)
        {
            Attachments.Add(item: path);
            ChangeDisplayedAttahmentCount();
            AttachmentsToolTip = GetToolTip();
        }

        public Command AddAttachments => _addAttachments.Value;
        public Command SendMessage => _sendMessage.Value;

        private async Task UsersFilter()
        {
            if (SelectedUser?.DisplayedName == Filter)
                return;

            SelectedUser = null;
            Users = Filter.Length < 3 ? null : await User.GetReceivers(filter: Filter);
        }

        private void ChangeDisplayedAttahmentCount()
        {
            string loadedForm = Attachments.Count % 10 == 1 && Attachments.Count != 11 ? "Загружен" : "Загружено";
            string fileForm = WordFormulator.GetForm(count:Attachments.Count, forms: new string[] { "файлов", "файл", "файла" });
            DisplayedAttachmentsCount = $"{loadedForm} {Attachments.Count} {fileForm}";
        }

        public string GetToolTip()
        {
            if (Attachments.Count > 0)
                return $"[{String.Join(separator: ", ", values: Attachments.Select(f => new FileInfo(fileName: f).Name))}]";
            return "Файлы не загружены";
        }
    }
}
