using ElectronicJournal.Utilities.Validator;
using ElectronicJournal.ViewModels.Tools;
using System;
using System.ComponentModel.DataAnnotations;

namespace ElectronicJournal.Models
{
	public class AuthorizationModel : TrackedObject
	{
		private string _login = String.Empty;
		private string _password = String.Empty;

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

		public bool IsValid => ObjectValidator.Validate(instance: this);
	}
}
