using ElectronicJournal.Models;
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
    public class RegistrationVM : VM
    {
        #region Fields
        private readonly IValidator<RegistrationModel> _validator;
        private readonly IEventAggregator _eventAggregator;

        private RegistrationModel _model;

        private readonly Lazy<Command> _back;
        private readonly Lazy<Command> _registration;
        #endregion Fields

        #region Constructors
        public RegistrationVM(IValidator<RegistrationModel> validator, IEventAggregator eventAggregator)
            : base(defaultButtonContent: "Зарегистрироваться")
        {
            _validator = validator;
            _eventAggregator = eventAggregator;

            _model = new RegistrationModel();
            _model.PropertyChanged += (object sender, PropertyChangedEventArgs e) => OnPropertyChanged(propertyName: e.PropertyName);

            _back = Command.CreateLazyCommand(
                action: _ => _eventAggregator.GetEvent<ChangeMainWindowContentEvent>().Publish(payload: new ChangeMainWindowContentEventArgs { NewVM = Program.AppHost.Services.GetService<AuthorizationVM>() }),
                canExecute: _ => CanMoveToAnotherPage
            );

            _registration = Command.CreateLazyCommand(action: async _ =>
            {
                RegistrationModule rm = await ExecuteTask(taskForExecute: _model.VerifyRegistrationCodeAsync);
                RegistrationOfAuthorizationDataVM registrationOfAuthorizationDataVM = Program.AppHost.Services.GetService<RegistrationOfAuthorizationDataVM>();
                registrationOfAuthorizationDataVM.RegistrationModule = rm;
                _eventAggregator.GetEvent<ChangeMainWindowContentEvent>().Publish(payload: new ChangeMainWindowContentEventArgs { NewVM = registrationOfAuthorizationDataVM });
            },
            canExecute: _ => _validator.Validate(instance: _model).IsValid && CanMoveToAnotherPage);
        }
        #endregion Constructors

        #region Properties
        public Command Registration => _registration.Value;
        public Command Back => _back.Value;

        public string Code
        {
            get => _model.Code;
            set => _model.Code = value;
        }
        #endregion Properties
    }
}
