using System.Collections.Generic;
using System.Linq;
using Train2d.Main.Commands;
using Train2d.Main.Controls;

namespace Train2d.Main
{
  public class CommandController
  {
    #region Attributes

    private readonly List<CommandBase> _commands;
    private readonly List<CommandBase> _redoCommands;
    private readonly List<CommandBase> _newCommands;
    #endregion

    #region Construct

    public CommandController()
    {
      _commands = new List<CommandBase>();
      _redoCommands = new List<CommandBase>();
      _newCommands = new List<CommandBase>();
      UndoCommand = new DelegateCommand(UndoCommandExecute);
      RedoCommand = new DelegateCommand(RedoCommandExecute);
    }

    #endregion

    #region Methods

    public void AddCommandAndExecute(CommandBase newCommand)
    {
      AddCommand(newCommand);
      ExecuteNewCommands();
    }

    public void AddCommand(CommandBase newCommand)
    {
      _newCommands.Add(newCommand);
    }

    public void ExecuteNewCommands()
    {
      foreach (CommandBase newCommand in _newCommands)
      {
        newCommand.ExecuteAction();
        _commands.Add(newCommand);
      }
      _newCommands.Clear();
    }

    #endregion

    #region Commands

    public DelegateCommand UndoCommand { get; private set; }

    private void UndoCommandExecute(object o)
    {
      CommandBase lastCommand = _commands.LastOrDefault();
      if (lastCommand == null)
      {
        return;
      }
      _commands.RemoveAt(_commands.Count - 1);
      lastCommand.UndoAction();
      _redoCommands.Add(lastCommand);
    }

    public DelegateCommand RedoCommand { get; private set; }

    private void RedoCommandExecute(object o)
    {
      CommandBase lastUndoCommand = _redoCommands.LastOrDefault();
      if (lastUndoCommand == null)
      {
        return;
      }
      _redoCommands.RemoveAt(_redoCommands.Count - 1);
      lastUndoCommand.ExecuteAction();
      _commands.Add(lastUndoCommand);
    }

    #endregion

  }
}
