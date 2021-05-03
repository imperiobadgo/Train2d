using Train2d.Main.ViewModel;
using Train2d.Main.ViewModel.Items;

namespace Train2d.Main.Commands
{
  public class SetSignalStateCommand : CommandItemBase<SignalViewModel>
  {
    private int _oldState;
    private int _newState;

    public SetSignalStateCommand(LayoutViewModel viewModel, SignalViewModel signal, int newState) : base(viewModel, signal)
    {
      _oldState = signal.Item().State;
      _newState = newState;
    }

    protected override bool Execute()
    {
      _item.SetState(_newState);
      return true;
    }

    protected override void Undo()
    {
      _item.SetState(_oldState);
    }

    public override string ToString()
    {
      return $"Configured from {_oldState} to {_newState}. Type: {_item}";
    }
  }
}
