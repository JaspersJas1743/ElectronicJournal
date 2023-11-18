using ElectronicJournalAPI.ApiEntities;
using Prism.Events;
using System;

namespace ElectronicJournal.Utilities.PubSubEvents
{
    public class CloseMessageCreationAfterSendingEvent : PubSubEvent<CloseMessageCreationAfterSendingEventArgs>
    {
    }

    public class CloseMessageCreationAfterSendingEventArgs : EventArgs
    {
        public User User { get; set; }
    }
}
