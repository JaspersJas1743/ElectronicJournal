using System;
using System.Windows.Input;

namespace ElectronicJournal.ViewModels.Tools
{
    public class Command : ICommand
    {
        private Action<object> _execute;
        private Predicate<object> _canExecute;

        public Command(Action<object> execute, Predicate<object> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
            => _canExecute is null || _canExecute(parameter);

        public void Execute(object parameter)
            => _execute(obj: parameter);

        public static Lazy<Command> CreateLazyCommand(Action<object> action, Predicate<object> canExecute = null)
            => new Lazy<Command>(valueFactory: () => new Command(execute: action, canExecute: canExecute));
    }
}
