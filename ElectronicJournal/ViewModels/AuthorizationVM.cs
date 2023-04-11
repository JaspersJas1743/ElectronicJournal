using ElectronicJournal.Models;
using ElectronicJournal.Properties;
using ElectronicJournal.Utilities;
using ElectronicJournal.Views;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Windows;

namespace ElectronicJournal.ViewModels
{
	public class AuthorizationVM : BaseVM
	{
		private AuthorizationModel _model;
		private readonly Lazy<Command> _authorize;
		private readonly Lazy<Command> _moveToRegistration;
		private readonly Lazy<Command> _moveToPasswordRecovery;

		public AuthorizationVM()
		{
			_model = new AuthorizationModel();
			_authorize = Command.CreateLazyCommand(action: obj => MessageBox.Show($"Login={Login};Password={Password};\nToken={GenerateJWT()}"));
			_moveToRegistration = Command.CreateLazyCommand(action: obj => Navigation.Navigate<RegistrationVM>());
			_moveToPasswordRecovery = Command.CreateLazyCommand(action: obj => Navigation.Navigate<PasswordRecoveryVM>());
		}

		public string Login
		{
			get => _model.Login;
			set
			{
				_model.Login = value;
				OnPropertyChanged("Login");
			}
		}

        public string Password
        {
            get => _model.Password;
            set
            {
                _model.Password = value;
                OnPropertyChanged("Password");
            }
        }

		public Command Authorize => _authorize.Value;
		public Command MoveToRegistration => _moveToRegistration.Value;
		public Command MoveToPasswordRecovery => _moveToPasswordRecovery.Value;

		//private async Task CheckData(object obj)
		//{
		//	if (!ValidateModel(out string message))
		//	{
		//		MessageBox.Show(messageBoxText: message);
		//		return;
		//	}

		//	string tokenForUser = GenerateJWT();
		//	User user = await Context.Users.FirstOrDefaultAsync(predicate: x => x.AuthKey == tokenForUser);
		//	if (user is null)
		//	{
		//		MessageBox.Show("Неверные аутентификационные данные");
		//	}
		//	else
		//		Navigation.Navigate(page: new Timetable());
		//}

        private bool ValidateModel(out string message)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            bool result = Validator.TryValidateObject(
                instance: _model, validationContext: new ValidationContext(instance: _model),
                validationResults: results, validateAllProperties: true
            );
            message = String.Join(separator: "\n", values: results.Select(x => x.ErrorMessage));
            return result;
        }

        public string GenerateJWT()
        {
            string issuer = Settings.Default.Issuer;
            string audience = Settings.Default.Audience;
            Claim[] claims = new[]
            {
                new Claim(type: JwtRegisteredClaimNames.Name, value: Login + Password),
                new Claim(type: JwtRegisteredClaimNames.Iss, value: issuer),
                new Claim(type: JwtRegisteredClaimNames.Aud, value: audience)
            };
            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(key: Encoding.UTF8.GetBytes(s: Settings.Default.SecurityKey));
            SigningCredentials signingCredentials = new SigningCredentials(key: secretKey, algorithm: SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken token = new JwtSecurityToken(
                issuer: issuer, audience: audience, claims: claims, signingCredentials: signingCredentials
            );
            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenAsString.Length <= 255 ? tokenAsString : tokenAsString.Substring(startIndex: 0, length: 255);
        }
    }
}
