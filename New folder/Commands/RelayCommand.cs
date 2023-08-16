using System;
using System.Windows.Input;

namespace WpfAppWithRedisCache.Commands
{
    /// <summary>
    /// A command that invokes a delegate.
    /// The command parameter must be of type T.
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        private readonly Predicate<T>? _canExecute;
        private readonly Action<T>? _execute;

        public RelayCommand(Action<T>? execute)
        {
            this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this._canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object? parameter)
        {
            if (parameter != null && parameter is not T)
                return false;
            return parameter is T param && (_canExecute == null || _canExecute(param));
        }

        public void Execute(object? parameter)
        {
            if (_execute is not null && parameter is T param)
            {
                _execute.Invoke(param);
            }
        }
    }
    /// <summary>
    /// A command that invokes a delegate.
    /// This class does not provide the command parameter to the delegate -
    /// if you need that, use the generic version of this class instead.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Func<bool>? _canExecute;
        private readonly Action? _execute;

        public RelayCommand(Action execute)
        {
            this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this._canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object? parameter)
        {
            _execute?.Invoke();
        }
    }
}

