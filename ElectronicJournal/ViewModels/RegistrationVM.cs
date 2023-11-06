using ElectronicJournal.Resources.CustomElements;
using ElectronicJournal.Resources.Windows;
using ElectronicJournal.Utilities.Navigation;
using ElectronicJournal.ViewModels.Tools;
using ElectronicJournalAPI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicJournal.ViewModels
{
    public class RegistrationVM : VM
    {
        #region Fields
        private readonly INavigationProvider _navigationProvider;

        private readonly Lazy<Command> _back;
        private readonly Lazy<Command> _registration;

        private string _code;
        private RegistrationModule _rm;
        #endregion Fields

        #region Constructors
        public RegistrationVM(INavigationProvider navigationProvider)
            : base(defaultButtonContent: "Зарегистрироваться")
        {
            _navigationProvider = navigationProvider;
            _back = Command.CreateLazyCommand(
                action: _ => _navigationProvider.MoveTo<MainWindowVM, AuthorizationVM>(),
                canExecute: _ => CanMoveToAnotherPage
            );

            _registration = Command.CreateLazyCommand(action: async _ =>
            {
                try
                {
                    if (await ExecuteTask(taskForExecute: VerifyRegistrationCodeAsync))
                    {
                        _navigationProvider.MoveTo<MainWindowVM, RegistrationOfAuthorizationDataVM>(parameters: new Dictionary<string, object>()
                        {
                            ["RegistrationModule"] = _rm
                        });
                    }
                    else
                        MessageWindow.ShowError(text: "Проверьте правильность кода или обратитесь в Вашу организацию");
                } catch (Exception ex)
                {
                    MessageWindow.ShowError(text: ex.Message);
                }
            },
            canExecute: _ => Code?.Length == CodeEntryPanel.MaxCountOfCell && CanMoveToAnotherPage);
        }
        #endregion Constructors

        #region Properties
        public Command Registration => _registration.Value;

        public Command Back => _back.Value;

        public string Code
        {
            get => _code;
            set
            {
                _code = value;
                OnPropertyChanged(propertyName: nameof(Code));
            }
        }
        #endregion Properties

        #region Classes
        public class RegistrationCodeResponse
        {
            public bool IsVerified { get; set; }
        }
        #endregion Classes

        #region Methods
        private async Task<bool> VerifyRegistrationCodeAsync()
        {
            try
            {
                _rm = await RegistrationModule.Create(registrationCode: Code);
                return true;
            } catch { return false; }
        }
        #endregion Methods
    }
}
