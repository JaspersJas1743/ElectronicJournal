using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;

namespace ElectronicJournal.ViewModels
{
    public class TimetableVM : VM
    {
        #region Fields
        //private readonly INavigationProvider _navigationProvider;

        #endregion Fields

        #region Constructor
        public TimetableVM()
        {
        }
        #endregion Constructor

        #region Properties
        public User User { get; set; }
        #endregion Properties
    }
}
