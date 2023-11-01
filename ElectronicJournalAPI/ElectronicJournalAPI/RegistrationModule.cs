﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicJournalAPI
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
        public class RegistrationCodeResponse
        {
            public bool IsVerified { get; set; }
        }

        public class SignUpRequest
        {
            public string RegistrationCode { get; set; }
            public string Login { get; set; }
            public string Password { get; set; }
        }
        #endregion Classes

        #region Methods
        public static async Task<RegistrationModule> Create(string registrationCode)
        {
            RegistrationModule rm = new RegistrationModule(registrationCode: registrationCode);
            await rm.Verify();
            return rm;
        }

        private async Task Verify()
        {
            RegistrationCodeResponse response = await ApiClient.GetAsync<RegistrationCodeResponse>(
                apiMethod: "Account/VerifyRegistrationCode", argQuery: new Dictionary<string, string> {
                    { "RegistrationCode", _registrationCode }
                }
            );

            if (!response.IsVerified)
                throw new ArgumentException(message: "Данный регистрационный код не является действительным :(", paramName: nameof(_registrationCode));
        }

        public async Task SignUpAsync(string login, string password)
        {
            SignUpRequest request = new SignUpRequest() { RegistrationCode = _registrationCode, Login = login, Password = password };
            await ApiClient.PostAsync<SignUpRequest>(apiMethod: "Account/SignUp", arg: request);
        }
        #endregion Methods
    }
}