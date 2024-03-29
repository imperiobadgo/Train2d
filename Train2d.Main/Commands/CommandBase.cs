﻿namespace Train2d.Main.Commands
{
  public abstract class CommandBase
  {
    private bool _isExecuted;
    public void ExecuteAction()
    {
      _isExecuted = Execute();
    }

    protected abstract bool Execute();

    public void UndoAction()
    {
      if (_isExecuted)
      {
        Undo();
      }
    }

    protected abstract void Undo();

    public string DisplayText
    {
      get => ToString();
    }
  }
}
