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

    public void Add(CommandBase command)
    {
      _commands.Add(command);
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

    public List<CommandBase> CommandsInChain
    {
      get => _commands;
    }

    public override string ToString()
    {
      return $"{_commands.Count} Items";
    }

  }
}
