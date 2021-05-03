using Train2d.Main.ViewModel;
using Train2d.Main.ViewModel.Items;
using Train2d.Model;

namespace Train2d.Main.Commands
{
  public class SetSignalDirectionCommand : CommandItemBase<SignalViewModel>
  {

    private int _newDirection;
    private int _oldDirection;

    public SetSignalDirectionCommand(LayoutViewModel viewModel, SignalViewModel signal, int direction) : base(viewModel, signal)
    {
      _newDirection = direction;
      _oldDirection = signal.Direction;
    }

    protected override bool Execute()
    {
      _item.SetDirection(_newDirection);
      return true;
    }

    protected override void Undo()
    {
      _item.SetDirection(_oldDirection);
    }

    public override string ToString()
    {
      return $"Set Direction of item to {_newDirection}. Type: {_item}";
    }

  }
}
