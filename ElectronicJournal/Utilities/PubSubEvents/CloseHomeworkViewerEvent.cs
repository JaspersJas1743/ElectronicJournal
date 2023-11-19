using ElectronicJournalAPI.ApiEntities;
using Prism.Events;
using System;

namespace ElectronicJournal.Utilities.PubSubEvents
{
    public class CloseHomeworkViewerEvent : PubSubEvent<CloseHomeworkViewerEventArgs>
    {

    }

    public class CloseHomeworkViewerEventArgs : EventArgs
    {
        public User User { get; set; }
    }
}
