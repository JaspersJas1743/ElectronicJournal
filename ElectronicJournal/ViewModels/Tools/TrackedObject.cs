using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ElectronicJournal.ViewModels.Tools
{
    public class TrackedObject : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion Events

        #region Methods
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(sender: this, e: new PropertyChangedEventArgs(propertyName: propertyName));
        #endregion Methods
    }
}
