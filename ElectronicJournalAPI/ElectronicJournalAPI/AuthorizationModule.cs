﻿using System.Threading.Tasks;

namespace ElectronicJournalAPI
{
    public class AuthorizationModule
    {
        #region Fields
        private readonly string _login;
        private readonly string _password;
        #endregion Fields

        #region Constructors
        private AuthorizationModule(string login, string password)
        {
            _login = login;
            _password = password;
        }
        #endregion Constructors

        #region Classes
        private class LogInResponse
        {
            public string Token { get; set; }
        }

        private class LogInRequest
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }
        #endregion Classes

        #region Methods
        public static AuthorizationModule Create(string login, string password)
            => new AuthorizationModule(login: login, password: password);

        public async Task SignInAsync()
        {
            LogInRequest request = new LogInRequest() { Login = _login, Password = _password };
            LogInResponse response = await ApiClient.PostAsync<LogInResponse, LogInRequest>(apiMethod: "Account/SignIn", arg: request);
            ApiClient.Token = response.Token;
        }
        #endregion Methods
    }
}
