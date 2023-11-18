using Prism.Events;
using System;

namespace ElectronicJournal.Utilities.PubSubEvents
{
    public class UserUploadPhotoEvent : PubSubEvent<UserUploadPhotoEventArgs>
    {
    }

    public class UserUploadPhotoEventArgs : EventArgs
    {
        public string NewPhoto { get; set; }
    }
}
