using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Train2d.Main.ViewModel;
using Train2d.Main.ViewModel.Items;

namespace Train2d.Main.Commands
{
  public class SetTrainForceStopStateCommand : CommandItemBase<TrainViewModel>
  {
    private bool _newState;
    private bool _oldState;

    public SetTrainForceStopStateCommand(LayoutViewModel viewModel, TrainViewModel train, bool state) : base(viewModel, train)
    {
      _newState = state;
      _oldState = train.ForceStop;
    }

    protected override bool Execute()
    {
      _item.ForceStop = _newState;
      return true;
    }

    protected override void Undo()
    {
      _item.ForceStop = _oldState;
    }

    public override string ToString()
    {
      return $"Set ForceStop of item from {_oldState} to {_newState}. Type: {_item}";
    }
  }
}
