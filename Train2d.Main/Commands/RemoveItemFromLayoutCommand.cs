using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Train2d.Main.ViewModel;
using Train2d.Main.ViewModel.Items;
using Train2d.Model;

namespace Train2d.Main.Commands
{
  class RemoveItemFromLayoutCommand : CommandItemBase<ItemViewModel>
  {
    private Coordinate _coordinate;

    public RemoveItemFromLayoutCommand(LayoutViewModel viewModel, ItemViewModel item) : base(viewModel, item)
    {
      _coordinate = item.Coordinate.Value;
    }

    protected override bool Execute()
    {
      return _viewModel.LayoutController.RemoveItemFromLayout(_coordinate, _item);
    }

    protected override void Undo()
    {
      _viewModel.LayoutController.AddItemToLayout(_coordinate, _item);
    }

    public override string ToString()
    {
      return $"Removed item from {_coordinate}. Type: {_item}";
    }
  }
}
