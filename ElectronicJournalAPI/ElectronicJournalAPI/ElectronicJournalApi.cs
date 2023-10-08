using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicJournalAPI
{
    public class ElectronicJournalApi
    {
        #region Fields
        private readonly string _login;
        private readonly string _password;
        private readonly string _registrationCode;

        private string _token;
        private bool _isVerifiedRegistrationCode;
        #endregion Fields

        #region Constructors
        public ElectronicJournalApi(string login = null, string password = null, string token = null)
        {
            _login = login;
            _password = password;
            _token = token;
        }

        public ElectronicJournalApi(string registrationCode) 
            => _registrationCode = registrationCode;
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

        public class RegistrationCodeResponse
        {
            public bool IsVerified { get; set; }
        }
        #endregion Classes

        #region Methods
        public async Task SignInAsync()
        {
            if (!String.IsNullOrEmpty(_token))
            {
                ApiClient.Token = _token;
                return;
            }

            LogInRequest request = new LogInRequest() { Login = _login, Password = _password };
            LogInResponse response = await ApiClient.PostAsync<LogInResponse, LogInRequest>(apiMethod: "Account/SignIn", arg: request);
            ApiClient.Token = response.Token;
        }

        public async Task<bool> VerifyRegistrationCode()
        {
            if (String.IsNullOrEmpty(_registrationCode))
                throw new ArgumentException(message: "Регистрационный код является обязательным полем.", paramName: nameof(_registrationCode));

            RegistrationCodeResponse response = await ApiClient.GetAsync<RegistrationCodeResponse>(
                apiMethod: "Account/VerifyRegistrationCode", argQuery: new Dictionary<string, string> {
                                { "RegistrationCode", _registrationCode }
                }
            );
            _isVerifiedRegistrationCode = response.IsVerified;
            return _isVerifiedRegistrationCode;
        }

        public async Task SignUpAsync(string login, string password)
        {
            if (!_isVerifiedRegistrationCode)
                throw new ArgumentException(message: "Регистрационный код не проверен.");

            LogInRequest request = new LogInRequest() { Login = login, Password = password };
            await ApiClient.PostAsync<LogInRequest>(apiMethod: "Account/SignUp", arg: request);
        }
        #endregion Methods
    }
}
