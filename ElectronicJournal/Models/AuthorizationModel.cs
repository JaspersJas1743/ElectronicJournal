using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using System.Threading.Tasks;

namespace ElectronicJournal.Models
{
    public class AuthorizationModel : TrackedObject
    {
        private string _login = default;
        private string _password = default;
        private bool _saveData = default;

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

        public bool SaveData
        {
            get => _saveData;
            set
            {
                _saveData = value;
                OnPropertyChanged(propertyName: nameof(SaveData));
            }
        }

        public async Task<User> SignInAsync()
        {
            AuthorizationModule api = AuthorizationModule.Create(login: Login, password: Password);
            return await api.SignInAsync();
        }
    }
}
