using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using System.Threading.Tasks;

namespace ElectronicJournal.Models
{
    public class RegistrationModel : TrackedObject
    {
        private string _code = default;

        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged(propertyName: nameof(Code));
            }
        }

        public async Task<RegistrationModule> VerifyRegistrationCodeAsync()
            => await RegistrationModule.Create(registrationCode: Code);
    }
}
