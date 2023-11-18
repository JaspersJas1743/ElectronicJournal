using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using System.Collections.Generic;

namespace ElectronicJournal.Models
{
    public class MessageCreationModel : TrackedObject
    {
        private IEnumerable<User.MessageReceiversResponse> _users;
        private string _filter;
        private User.MessageReceiversResponse _selectedUser;
        private List<string> _attachments = new List<string>();
        private string _text;
        private string _displayedAttachmentsCount;
        private string _attachmentsToolTip;

        public IEnumerable<User.MessageReceiversResponse> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(propertyName: nameof(Users));
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

        public User.MessageReceiversResponse SelectedUser
        {
            get => _selectedUser;
            set
            {
                _selectedUser = value;
                OnPropertyChanged(propertyName: nameof(SelectedUser));
            }
        }

        public List<string> Attachments
        {
            get => _attachments;
            set
            {
                _attachments = value;
                OnPropertyChanged(propertyName: nameof(Attachments));
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged(propertyName: nameof(Text));
            }
        }

        public string DisplayedAttachmentsCount
        {
            get => _displayedAttachmentsCount;
            set
            {
                _displayedAttachmentsCount = value;
                OnPropertyChanged(propertyName: nameof(DisplayedAttachmentsCount));
            }
        }

        public string AttachmentsToolTip
        {
            get => _attachmentsToolTip;
            set
            {
                _attachmentsToolTip = value;
                OnPropertyChanged(propertyName: nameof(AttachmentsToolTip));
            }
        }
    }
}
