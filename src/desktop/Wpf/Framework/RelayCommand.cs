using System;
using System.Windows.Input;

namespace Budgetter.Wpf.Framework;

public class RelayCommand : ICommand
{
    private readonly Func<object, bool> _canExecuteCallback;
    private readonly Action<object> _executeCallback;

    public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
    {
        _executeCallback = execute;
        _canExecuteCallback = canExecute;
    }

    public event EventHandler CanExecuteChanged
    {
        add => CommandManager.RequerySuggested += value;
        remove => CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object parameter)
    {
        return _canExecuteCallback == null || _canExecuteCallback(parameter);
    }

    public void Execute(object parameter)
    {
        _executeCallback(parameter);
    }
}