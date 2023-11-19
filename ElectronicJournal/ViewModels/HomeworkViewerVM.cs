using ElectronicJournal.Utilities.Config;
using ElectronicJournal.Utilities.PubSubEvents;
using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using Prism.Events;
using System;

namespace ElectronicJournal.ViewModels
{
    public class HomeworkViewerVM : VM
    {
        private IEventAggregator _eventAggregator;
        private IConfigProvider _config;

        private Lazy<Command> _goBack;
        private Lazy<Command> _downloadAttachment;

        public HomeworkViewerVM(IEventAggregator eventAggregator, IConfigProvider config)
        {
            _eventAggregator = eventAggregator;
            _config = config;

            _goBack = Command.CreateLazyCommand(action: _ =>
            {
                CloseHomeworkViewerEventArgs e = new CloseHomeworkViewerEventArgs
                {
                    User = User
                };
                _eventAggregator.GetEvent<CloseHomeworkViewerEvent>().Publish(payload: e);
            });
            _downloadAttachment = Command.CreateLazyCommand(action: async _ => await Homework.Attachment?.Download(folder: config.Get<string>(propertyName: "FolderForDownloads")));
        }

        public User User { get; set; }
        public Homework Homework { get; set; }

        public Command GoBack => _goBack.Value;
        public Command DownloadAttachment => _downloadAttachment.Value;
    }
}
