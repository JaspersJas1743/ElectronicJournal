using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ElectronicJournalAPI.ApiEntities
{
    public class RegistrationModule
    {
        #region Fields
        private string _registrationCode;
        #endregion Fields

        #region Contructors
        public RegistrationModule(string registrationCode)
        {
            if (registrationCode is null)
                throw new ArgumentNullException(message: "Регистрационный код не может быть пустым.", paramName: nameof(registrationCode));

            _registrationCode = registrationCode;
        }
        #endregion Contructors

        #region Classes
        private class RegistrationCodeResponse
        {
            public bool IsVerified { get; set; }
        }

        private class SignUpRequest
        {
            public string RegistrationCode { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
        }
        #endregion Classes

        #region Methods
        public static async Task<RegistrationModule> Create(string registrationCode, CancellationToken cancellationToken = default)
        {
            RegistrationModule rm = new RegistrationModule(registrationCode: registrationCode);
            await rm.Verify(cancellationToken: cancellationToken);
            return rm;
        }

        private async Task Verify(CancellationToken cancellationToken = default)
        {
            RegistrationCodeResponse response = await ApiClient.GetAsync<RegistrationCodeResponse>(
                apiMethod: "Account/VerifyRegistrationCode",
                argQuery: new Dictionary<string, string> {
                    { "RegistrationCode", _registrationCode }
                },
                cancellationToken: cancellationToken
            );

            if (!response.IsVerified)
                throw new ArgumentException(message: "Данный регистрационный код не является действительным :(");
        }

        public async Task SignUpAsync(string login, string password, CancellationToken cancellationToken = default)
        {
            SignUpRequest request = new SignUpRequest() { RegistrationCode = _registrationCode, Login = login, Password = password };
            await ApiClient.PostAsync(
                apiMethod: "Account/SignUp",
                arg: request,
                cancellationToken: cancellationToken
            );
        }
        #endregion Methods
    }
}
