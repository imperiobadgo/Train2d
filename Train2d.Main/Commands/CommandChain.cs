using System.Collections.Generic;

namespace Train2d.Main.Commands
{
  public class CommandChain : CommandBase
  {
    private List<CommandBase> _commands;

    public CommandChain(List<CommandBase> commands)
    {
      _commands = commands;
    }

    protected override bool Execute()
    {
      for (int i = 0; i < _commands.Count; i++)
      {
        _commands[i].ExecuteAction();
      }
      return true;
    }

    protected override void Undo()
    {
      for (int i = _commands.Count - 1; i >= 0; i--)
      {
        _commands[i].UndoAction();
      }
    }
  }
}
