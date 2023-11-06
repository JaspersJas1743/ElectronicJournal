﻿using ElectronicJournal.ViewModels.Tools;
using System;

namespace ElectronicJournal.Models
{
    public class RegistrationOfAuthorizationDataModel : TrackedObject
    {
        private string _login = String.Empty;
        private string _password = String.Empty;
        private string _passwordСonfirmation = String.Empty;

        public string Login
        {
            get => _login;
            set
            {
                _login = value;
                OnPropertyChanged(propertyName: nameof(Login));
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(propertyName: nameof(Password));
            }

        }

        public string PasswordConfirmation
        {
            get => _passwordСonfirmation;
            set
            {
                _passwordСonfirmation = value;
                OnPropertyChanged(propertyName: nameof(PasswordConfirmation));
            }

        }
    }
}
