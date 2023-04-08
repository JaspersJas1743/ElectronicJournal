using ElectronicJournal.Entities;
using ElectronicJournal.Models;
using ElectronicJournal.Properties;
using ElectronicJournal.Utilities;
using ElectronicJournal.Views;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronicJournal.ViewModels
{
	public class AuthorizationVM : INotifyPropertyChanged
	{
		private AuthorizationModel _model;
		private readonly Lazy<Command> _authorize;
		private readonly Lazy<Command> _moveToRegistration;
		private readonly Lazy<Command> _moveToPasswordRecovery;
		private readonly Lazy<ElectronicJournalEntities> _context;

		public AuthorizationVM()
		{
			_model = new AuthorizationModel();
			_authorize = CreateCommand(action: async (obj) => await CheckData(obj));
			_moveToRegistration = CreateCommand(action: obj => Navigation.Navigate(page: new Registration()));
			_moveToPasswordRecovery = CreateCommand(action: obj => Navigation.Navigate(page: new PasswordRecovery()));
			_context = new Lazy<ElectronicJournalEntities>(valueFactory: () => new ElectronicJournalEntities());
		}

		~AuthorizationVM()
		{
			if (_context.IsValueCreated)
				_context.Value.Dispose();
		}

		public event PropertyChangedEventHandler PropertyChanged;

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
		private ElectronicJournalEntities Context => _context.Value;

		private Lazy<Command> CreateCommand(Action<object> action)
			=> new Lazy<Command>(valueFactory: () => new Command(execute: action));

		public void OnPropertyChanged([CallerMemberName] string prop = "")
			=> PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

		private async Task CheckData(object obj)
		{
			if (!ValidateModel(out string message))
			{
				MessageBox.Show(messageBoxText: message);
				return;
			}

			string tokenForUser = GenerateJWT();
			User user = await Context.Users.FirstOrDefaultAsync(predicate: x => x.AuthKey == tokenForUser);
			if (user is null)
			{
				MessageBox.Show("Неверные аутентификационные данные");
			}
			else
				Navigation.Navigate(page: new Timetable());
		}

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
