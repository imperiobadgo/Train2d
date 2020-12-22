using System;
using System.Windows.Input;

namespace Train2d.Main.Controls
{
  public class DelegateCommand : ICommand
  {
    public event EventHandler CanExecuteChanged;

    private readonly Action<object> _executeHandler;
    private readonly Func<object, bool> _canExecuteHandler;
    private bool _canExecuteCache;
    //  Private ReadOnly _executeHandler As Action(Of Object)
    //Private ReadOnly _canExecuteHandler As Predicate(Of Object)

    public DelegateCommand(Action<object> execute)
    {
      _executeHandler = execute;
    }

    public DelegateCommand(Action<object> execute, Func<object, bool> canExecute)
    {
      _executeHandler = execute;
      _canExecuteHandler = canExecute;
    }

    public bool CanExecute(object parameter)
    {
      if (_canExecuteHandler == null)
        return true;

      var canExec = _canExecuteHandler(parameter);
      if ((canExec != _canExecuteCache))
      {
        _canExecuteCache = canExec;
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
      }

      return canExec;
    }

    public void RaiseCanExecuteChanged()
    {
      CanExecute(null);
    }

    public void Execute(object parameter)
    {
      Mouse.OverrideCursor = Cursors.Wait;
      Mouse.UpdateCursor();

      _executeHandler.Invoke(parameter);

      Mouse.OverrideCursor = null;
    }
  }
}
