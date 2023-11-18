using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using System.Threading.Tasks;

namespace ElectronicJournal.Models
{
    public class RegistrationOfAuthorizationDataModel : TrackedObject
    {
        private string _login = default;
        private string _password = default;
        private string _passwordСonfirmation = default;

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(propertyName: nameof(Login));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(propertyName: nameof(Password));
            }
        }

        public string PasswordConfirmation
        {
            get => _passwordСonfirmation;
            set
            {
                _passwordСonfirmation = value;
                OnPropertyChanged(propertyName: nameof(PasswordConfirmation));
            }
        }

        public async Task SignUpAsync(RegistrationModule registrationModule)
            => await registrationModule.SignUpAsync(login: Login, password: Password);
    }
}
