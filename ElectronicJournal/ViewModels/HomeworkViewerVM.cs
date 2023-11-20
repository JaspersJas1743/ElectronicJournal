using ElectronicJournal.Resources.Windows;
using ElectronicJournal.Utilities.Config;
using ElectronicJournal.Utilities.Messages;
using ElectronicJournal.Utilities.PubSubEvents;
using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using Prism.Events;
using System;
using System.Diagnostics;

namespace ElectronicJournal.ViewModels
{
    public class HomeworkViewerVM : VM
    {
        private IEventAggregator _eventAggregator;

        private IConfigProvider _config;
        private IMessageProvider _message;

        private Lazy<Command> _goBack;
        private Lazy<Command> _downloadAttachment;

        public HomeworkViewerVM(IEventAggregator eventAggregator, IConfigProvider config, IMessageProvider message)
        {
            _eventAggregator = eventAggregator;
            _config = config;
            _message = message;

            _goBack = Command.CreateLazyCommand(action: _ =>
            {
                CloseHomeworkViewerEventArgs e = new CloseHomeworkViewerEventArgs
                {
                    User = User
                };
                _eventAggregator.GetEvent<CloseHomeworkViewerEvent>().Publish(payload: e);
            });
            _downloadAttachment = Command.CreateLazyCommand(action: async _ =>
            {
                await Homework.Attachment.Download(folder: _config.Get<string>(propertyName: "FolderForDownloads"));
                MessageWindow.MessageWindowResult result = _message.Show(
                    text: "Файл сохранён! Открыть?",
                    windowTitle: String.Empty,
                    image: MessageWindow.MessageWindowImage.Information,
                    buttons: MessageWindow.MessageWindowButton.YesNo);
                if (result.Equals(MessageWindow.MessageWindowResult.Yes))
                    Process.Start(fileName: Homework.Attachment.Path);
            });
        }

        public User User { get; set; }
        public Homework Homework { get; set; }

        public Command GoBack => _goBack.Value;
        public Command DownloadAttachment => _downloadAttachment.Value;
    }
}
