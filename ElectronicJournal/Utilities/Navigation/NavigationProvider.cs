using ElectronicJournal.ViewModels;
using ElectronicJournal.ViewModels.Tools;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace ElectronicJournal.Utilities.Navigation
{
    public class NavigationProvider : INavigationProvider
    {
        public void MoveTo<OldPage, NewPage>(Dictionary<string, object> parameters = null) where NewPage : VM
                                                                                           where OldPage : ContentPresenter
        {
            OldPage vm = Program.AppHost.Services.GetService<OldPage>();
            NewPage newPage = Program.AppHost.Services.GetService<NewPage>();
            foreach (var parameter in parameters ?? new Dictionary<string, object>())
                typeof(NewPage).GetProperty(name: parameter.Key).SetValue(obj: newPage, value: parameter.Value);
            vm.Content = newPage;
        }
    }
}
