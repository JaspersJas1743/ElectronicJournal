using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace ElectronicJournal.Models
{
    public class UpdMessage
    {
        private readonly Lazy<Command> _downloadAttachment;

        public UpdMessage(Message message)
        {
            Message = message;
            _downloadAttachment = Command.CreateLazyCommand(action: async _ =>
            {
                await Message.Attachment.Download(folder: Properties.Settings.Default.FolderForDownloads);
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
        private List<UpdMessage> _inboundMessages = new List<UpdMessage>();
        private List<UpdMessage> _outboundMessages = new List<UpdMessage>();
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

        public List<UpdMessage> InboundMessages
        {
            get => _inboundMessages;
            set
            {
                _inboundMessages = value;
                OnPropertyChanged(propertyName: nameof(InboundMessages));
            }
        }

        public List<UpdMessage> OutboundMessages
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
