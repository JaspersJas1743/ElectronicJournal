using ElectronicJournal.Resources.Windows;
using ElectronicJournal.Utilities.Messages;
using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Controls;

namespace ElectronicJournal.Models
{
    public class UpdMessage
    {
        private IMessageProvider _message;

        private readonly Lazy<Command> _downloadAttachment;

        public UpdMessage(Message message, IMessageProvider messageProvider)
        {
            Message = message;
            _message = messageProvider;
            _downloadAttachment = Command.CreateLazyCommand(action: async _ =>
            {
                await Message.Attachment.Download(folder: Properties.Settings.Default.FolderForDownloads);
                MessageWindow.MessageWindowResult result = _message.Show(
                    text: "Файл сохранён! Открыть?",
                    windowTitle: String.Empty,
                    image: MessageWindow.MessageWindowImage.Information,
                    buttons: MessageWindow.MessageWindowButton.YesNo);
                if (result.Equals(MessageWindow.MessageWindowResult.Yes))
                    Process.Start(fileName: Message.Attachment.Path);
            }, canExecute: _ => Message.Attachment != null);
        }

        public Message Message { get; set; }
        public Command DownloadAttachment => _downloadAttachment.Value;
    }

    public class MessagesModel : TrackedObject
    {
        private IEnumerable<User.MessageReceiversResponse> _users;
        private User.MessageReceiversResponse _selectedUser;
        private UpdMessage _selectedMessage;
        private string _filter;
        private IEnumerable<UpdMessage> _inboundMessages;
        private IEnumerable<UpdMessage> _outboundMessages;
        private TabItem _selectedTabItem;

        public IEnumerable<User.MessageReceiversResponse> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(propertyName: nameof(Users));
            }
        }

        public User.MessageReceiversResponse SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(propertyName: nameof(SelectedUser));
            }
        }

        public string Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                OnPropertyChanged(propertyName: nameof(Filter));
            }
        }

        public UpdMessage SelectedMessage
        {
            get => _selectedMessage;
            set
            {
                _selectedMessage = value;
                OnPropertyChanged(propertyName: nameof(SelectedMessage));
            }
        }

        public IEnumerable<UpdMessage> InboundMessages
        {
            get => _inboundMessages;
            set
            {
                _inboundMessages = value;
                OnPropertyChanged(propertyName: nameof(InboundMessages));
            }
        }

        public IEnumerable<UpdMessage> OutboundMessages
        {
            get => _outboundMessages;
            set
            {
                _outboundMessages = value;
                OnPropertyChanged(propertyName: nameof(OutboundMessages));
            }
        }

        public TabItem SelectedTabItem
        {
            get => _selectedTabItem;
            set
            {
                _selectedTabItem = value;
                OnPropertyChanged(propertyName: nameof(SelectedTabItem));
            }
        }
    }
}
