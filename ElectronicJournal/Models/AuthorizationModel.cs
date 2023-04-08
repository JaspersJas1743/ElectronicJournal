using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace ElectronicJournal.Models
{
	public class AuthorizationModel: INotifyPropertyChanged
	{
		private string _login;
		private string _password;

		[Required(ErrorMessage = "Поле \"Логин\"является обязательным")]
		[MinLength(length: 4, ErrorMessage = "Минимальная длина логина - 4 символа")]
		public string Login 
		{
			get => _login;
			set
			{
				_login = value;
				OnPropertyChanged("Login");
			}
		}

		[Required(ErrorMessage = "Поле \"Пароль\"является обязательным")]
		[MinLength(length: 6, ErrorMessage = "Минимальная длина пароля - 6 символов")]
		public string Password
		{
			get => _password;
			set
			{
				_password = value;
				OnPropertyChanged("Password");
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string prop = "")
			=> PropertyChanged?.Invoke(sender: this, e: new PropertyChangedEventArgs(propertyName: prop));
	}
}
