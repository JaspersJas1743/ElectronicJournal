using ElectronicJournal.Utilities.Navigation;
using ElectronicJournal.ViewModels.Tools;

namespace ElectronicJournal.ViewModels
{
    public class TimetableVM : VM
    {
        #region Fields
        private readonly INavigationProvider _navigationProvider;

        #endregion Fields

        #region Constructor
        public TimetableVM(INavigationProvider navigationProvider)
        {
            _navigationProvider = navigationProvider;
        }
        #endregion Constructor

        #region Properties
        #endregion Properties
    }
}
