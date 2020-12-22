using System.Collections.Generic;
using System.Linq;
using Train2d.Main.Commands;

namespace Train2d.Main
{
  public class CommandController
  {
    #region Attributes

    private readonly List<CommandBase> _commands;

    #endregion

    #region Construct

    public CommandController()
    {
      _commands = new List<CommandBase>();
    }

    #endregion

    #region Methods

    public void AddCommandAndExecute(CommandBase newCommand)
    {
      AddCommand(newCommand);
      newCommand.ExecuteAction();
    }

    public void AddCommand(CommandBase newCommand)
    {
      _commands.Add(newCommand);
    }

    public CommandBase GetLastCommand()
    {
      return _commands.Last();
    }

    #endregion


  }
}
