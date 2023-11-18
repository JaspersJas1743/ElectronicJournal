using ElectronicJournalAPI.ApiEntities;
using Prism.Events;
using System;

namespace ElectronicJournal.Utilities.PubSubEvents
{
    public class ChangeMessageToCreationMessageEvent : PubSubEvent<ChangeMessageToCreationMessageEventArgs>
    {

    }

    public class ChangeMessageToCreationMessageEventArgs : EventArgs
    {
        public Message Message { get; set; }
        public User User { get; set; }
    }
}
