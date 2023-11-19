using ElectronicJournal.Models;
using ElectronicJournal.Utilities.Config;
using ElectronicJournal.Utilities.Logger;
using ElectronicJournal.Utilities.Messages;
using ElectronicJournal.Utilities.PubSubEvents;
using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ElectronicJournal.ViewModels
{
    public class MessagesVM : VM
    {
        private readonly IMessageProvider _message;
        private readonly IConfigProvider _config;
        private readonly ILoggerProvider _logger;
        private readonly IEventAggregator _eventAggregator;

        private MessagesModel _model;
        private int _offsetLoadedMessages;

        private readonly Lazy<Command> _send;
        private readonly Lazy<Command> _reply;
        private readonly Lazy<Command> _forward;
        private readonly Lazy<Command> _loadMoreMessages;
        private readonly Lazy<Command> _loaded;

        public MessagesVM(IMessageProvider message, IConfigProvider config, ILoggerProvider logger, IEventAggregator eventAggregator)
        {
            _message = message;
            _config = config;
            _logger = logger;
            _eventAggregator = eventAggregator;

            _model = new MessagesModel();

            _model.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(propertyName: e.PropertyName);

            _send = Command.CreateLazyCommand(action: _ =>
            {
                ChangeMessageToCreationMessageEventArgs e = new ChangeMessageToCreationMessageEventArgs
                {
                    User = User
                };
                _eventAggregator.GetEvent<ChangeMessageToCreationMessageEvent>().Publish(payload: e);
            });

            _reply = Command.CreateLazyCommand(action: _ =>
            {
                Message replyMessage = SelectedMessage.Message;
                replyMessage.Text = String.Empty;
                replyMessage.Attachment = null;
                ChangeMessageToCreationMessageEventArgs e = new ChangeMessageToCreationMessageEventArgs
                {
                    User = User,
                    Message = replyMessage
                };
                _eventAggregator.GetEvent<ChangeMessageToCreationMessageEvent>().Publish(payload: e);
            });

            _forward = Command.CreateLazyCommand(action: _ =>
            {
                Message forwardMessage = SelectedMessage.Message;
                forwardMessage.Text = $"Пересланное сообщение от {SelectedMessage.Message.Date}\n" +
                                      $"Отправитель: {SelectedMessage.Message.Sender}\n" +
                                      $"Получатель: {SelectedMessage.Message.Receiver}\n" +
                                      $"Текст сообщения:\n{SelectedMessage.Message.Text ?? "[Текст сообщения отсутствует]"}";
                forwardMessage.Sender = null;
                forwardMessage.SenderId = -1;
                ChangeMessageToCreationMessageEventArgs e = new ChangeMessageToCreationMessageEventArgs
                {
                    User = User,
                    Message = forwardMessage
                };
                _eventAggregator.GetEvent<ChangeMessageToCreationMessageEvent>().Publish(payload: e);
            });

            _loadMoreMessages = Command.CreateLazyCommand(action: async _ =>
            {
                await UpdateMessages(offset: _offsetLoadedMessages, count: 10);
            });

            _loaded = Command.CreateLazyCommand(action: obj => (obj as TabControl).SelectedIndex = SelectedIndex);
        }

        public User User { get; set; }

        public IEnumerable<User.MessageReceiversResponse> Users
        {
            get => _model.Users;
            set => _model.Users = value;
        }

        public User.MessageReceiversResponse SelectedUser
        {
            get => _model.SelectedUser;
            set
            {
                _model.SelectedUser = value;
                UpdateMessages(offset: 0, count: 20);
            }
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

        public UpdMessage SelectedMessage
        {
            get => _model.SelectedMessage;
            set => _model.SelectedMessage = value;
        }

        public List<UpdMessage> InboundMessages
        {
            get => _model.InboundMessages;
            set => _model.InboundMessages = value;
        }

        public List<UpdMessage> OutboundMessages
        {
            get => _model.OutboundMessages;
            set => _model.OutboundMessages = value;
        }

        public TabItem SelectedTabItem
        {
            get => _model.SelectedTabItem;
            set
            {
                _model.SelectedTabItem = value;
                UpdateMessages(offset: 0, count: 20);
                SelectedMessage = null;
            }
        }

        public int SelectedIndex { get; set; } = 0;

        public Command Send => _send.Value;
        public Command Reply => _reply.Value;
        public Command Forward => _forward.Value;
        public Command LoadMoreMessages => _loadMoreMessages.Value;
        public Command Loaded => _loaded.Value;

        private async Task UsersFilter()
        {
            if (SelectedUser?.DisplayedName == Filter)
                return;

            SelectedUser = null;
            Users = Filter.Length < 3 ? null : await User.GetReceivers(filter: Filter);
        }

        private async Task UpdateMessages(int offset, int count)
        {
            string dest = SelectedTabItem?.Name.ToString();
            if (string.IsNullOrEmpty(dest))
                return;

            IEnumerable<UpdMessage> messages = (await User.GetMessages(dest: dest, offset: offset, count: count, userId: SelectedUser?.Id ?? 0))
                .Select(m => new UpdMessage(message: m));

            if (offset == 0)
            {
                _offsetLoadedMessages = offset;
                InboundMessages = new List<UpdMessage>();
                OutboundMessages = new List<UpdMessage>();
            }

            _offsetLoadedMessages += messages.Count();

            if (nameof(InboundMessages).Contains(dest))
                InboundMessages.AddRange(collection: messages);
            else
                OutboundMessages.AddRange(collection: messages);
        }
    }
}
