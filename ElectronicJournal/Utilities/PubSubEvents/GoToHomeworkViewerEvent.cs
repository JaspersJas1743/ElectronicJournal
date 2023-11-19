using ElectronicJournalAPI.ApiEntities;
using Prism.Events;
using System;

namespace ElectronicJournal.Utilities.PubSubEvents
{
    public class GoToHomeworkViewerEvent : PubSubEvent<GoToHomeworkViewerEventArgs>
    {

    }

    public class GoToHomeworkViewerEventArgs : EventArgs
    {
        public Homework Homework { get; set; }
        public User User { get; set; }
    }
}
