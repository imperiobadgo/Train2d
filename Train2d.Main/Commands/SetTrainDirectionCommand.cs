using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Train2d.Main.ViewModel;
using Train2d.Main.ViewModel.Items;

namespace Train2d.Main.Commands
{
  class SetTrainDirectionCommand : CommandItemBase<TrainViewModel>
  {
    private int _newDirection;
    private int _oldDirection;

    public SetTrainDirectionCommand(LayoutViewModel viewModel, TrainViewModel train, int direction) : base(viewModel, train)
    {
      _newDirection = direction;
      _oldDirection = train.Direction;
    }

    protected override bool Execute()
    {
      _item.Direction = _newDirection;
      return true;
    }

    protected override void Undo()
    {
      _item.Direction = _oldDirection;
    }

  }
}
