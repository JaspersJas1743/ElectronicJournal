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
            => _canExecute is null || _canExecute(obj: parameter);

        public void Execute(object parameter)
            => _execute(obj: parameter);

        public static Lazy<Command> CreateLazyCommand(Action<object> action, Predicate<object> canExecute = null)
            => new Lazy<Command>(valueFactory: () => new Command(execute: action, canExecute: canExecute));
    }

    public class Command<T> : ICommand
    {
        private Action<T> _execute;
        private Predicate<T> _canExecute;

        public Command(Action<T> execute, Predicate<T> canExecute = null)
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
            => _canExecute is null || _canExecute(obj: (T)parameter);

        public void Execute(object parameter)
            => _execute(obj: (T)parameter);

        public static Lazy<Command<T>> CreateLazyCommand(Action<T> action, Predicate<T> canExecute = null)
            => new Lazy<Command<T>>(valueFactory: () => new Command<T>(execute: action, canExecute: canExecute));
    }
}
