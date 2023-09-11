using ElectronicJournal.Utilities.Validator;
using ElectronicJournal.ViewModels.Tools;
using System;
using System.ComponentModel.DataAnnotations;

namespace ElectronicJournal.Models.ForPage
{
	public class RegistrationOfAuthorizationDataModel : TrackedObject
	{
		private string _login = String.Empty;
		private string _password = String.Empty;
		private string _passwordСonfirmation = String.Empty;

		[Required(ErrorMessage = "Поле \"Логин\"является обязательным")]
		[MinLength(length: 4, ErrorMessage = "Минимальная длина логина - 4 символа")]
		public string Login
		{
			get => _login;
			set
			{
				_login = value;
				OnPropertyChanged(nameof(Login));
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
				OnPropertyChanged(nameof(Password));
			}

		}

		[Required(ErrorMessage = "Поле \"Пароль\"является обязательным")]
		[Compare(otherProperty: nameof(Password), ErrorMessage = "Пароли не совпадают")]
		public string PasswordConfirmation
		{
			get => _passwordСonfirmation;
			set
			{
				_passwordСonfirmation = value;
				OnPropertyChanged(nameof(PasswordConfirmation));
			}

		}

		public bool IsValid => ObjectValidator.Validate(instance: this);
	}
}
