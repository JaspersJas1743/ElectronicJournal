using ElectronicJournal.Models;
using ElectronicJournal.Utilities.Messages;
using ElectronicJournal.Utilities.PubSubEvents;
using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI.ApiEntities;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Prism.Events;
using System;
using System.ComponentModel;

namespace ElectronicJournal.ViewModels
{
    public class RegistrationOfAuthorizationDataVM : VM
    {
        #region Fields
        private readonly IValidator<RegistrationOfAuthorizationDataModel> _validator;
        private readonly IMessageProvider _message;
        private readonly IEventAggregator _eventAggregator;

        private RegistrationOfAuthorizationDataModel _model;

        private readonly Lazy<Command> _registration;
        private readonly Lazy<Command> _back;
        #endregion Fields

        #region Constructors
        public RegistrationOfAuthorizationDataVM(
            IValidator<RegistrationOfAuthorizationDataModel> validator,
            IMessageProvider message,
            IEventAggregator eventAggregator)
        {
            _validator = validator;
            _message = message;
            _eventAggregator = eventAggregator;

            _model = new RegistrationOfAuthorizationDataModel();
            _model.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(propertyName: e.PropertyName);

            _registration = Command.CreateLazyCommand(action: async _ =>
            {
                await ExecuteTask(taskForExecute: () => _model.SignUpAsync(registrationModule: RegistrationModule));
                _message.ShowInformation(text: "Регистрация прошла успешно!");
                ChangeMainWindowContentEventArgs e = new ChangeMainWindowContentEventArgs
                {
                    NewVM = Program.AppHost.Services.GetService<AuthorizationVM>()
                };
                _eventAggregator.GetEvent<ChangeMainWindowContentEvent>().Publish(payload: e);                
            }, canExecute: _ => _validator.Validate(instance: _model).IsValid && CanMoveToAnotherPage);

            _back = Command.CreateLazyCommand(
                action: _ => _eventAggregator.GetEvent<ChangeMainWindowContentEvent>().Publish(payload: new ChangeMainWindowContentEventArgs { NewVM = Program.AppHost.Services.GetService<AuthorizationVM>() }),
                canExecute: _ => CanMoveToAnotherPage
            );
        }
        #endregion Constructors

        #region Classes
        private class AuthData
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }
        #endregion Classes

        #region Properties
        public string Login
        {
            get => _model.Login;
            set => _model.Login = value;
        }

        public string Password
        {
            get => _model.Password;
            set => _model.Password = value;
        }

        public string PasswordConfirmation
        {
            get => _model.PasswordConfirmation;
            set => _model.PasswordConfirmation = value;
        }

        public Command Registration => _registration.Value;
        public Command Back => _back.Value;
        public RegistrationModule RegistrationModule { get; set; }
        #endregion Properties
    }
}
