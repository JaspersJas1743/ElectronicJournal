using ElectronicJournal.ViewModels.Tools;
using System.Collections.Generic;

namespace ElectronicJournal.Utilities.Navigation
{
    public interface INavigationProvider
    {
        void MoveTo<OldPage, NewPage>(Dictionary<string, object> parameters = null) where NewPage : VM
                                                                                    where OldPage : ContentPresenter;
    }
}
