using ElectronicJournal.ViewModels.Tools;
using Prism.Events;
using System;

namespace ElectronicJournal.Utilities.PubSubEvents
{
    public class ChangeMainWindowContentEvent : PubSubEvent<ChangeMainWindowContentEventArgs>
    { }

    public class ChangeMainWindowContentEventArgs : EventArgs
    {
        public VM NewVM { get; set; }
    }
}
