using ElectronicJournal.Utilities;
using ElectronicJournal.ViewModels;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace ElectronicJournal.Models
{
	public class AuthorizationModel : TrackedObject
	{
		private string _login;
		private string _password;

		public AuthorizationModel(string login, string password)
		{
			_login = login;
			_password = password;
		}

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

		public bool LoginIsValid
			=> ObjectValidator.ValidateProperty(instance: this, property: Login, propName: nameof(Login));

		public bool PasswordIsValid
			=> ObjectValidator.ValidateProperty(instance: this, property: Password, propName: nameof(Password));

		public bool IsValid => ObjectValidator.Validate(instance: this);
	}
}
