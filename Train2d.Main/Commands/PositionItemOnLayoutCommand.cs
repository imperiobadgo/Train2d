using Train2d.Main.ViewModel;
using Train2d.Main.ViewModel.Items;
using Train2d.Model;

namespace Train2d.Main.Commands
{
  class PositionItemOnLayoutCommand : CommandItemBase<ItemViewModel>
  {
    private Coordinate _coordinate;

    public PositionItemOnLayoutCommand(LayoutViewModel viewModel, ItemViewModel item, Coordinate coordinate) : base(viewModel, item)
    {
      _coordinate = coordinate;
    }

    protected override bool Execute()
    {
      return _viewModel.LayoutController.AddItemToLayout(_coordinate, _item);
    }

    protected override void Undo()
    {
      _viewModel.LayoutController.RemoveItemFromLayout(_coordinate, _item);
    }
  }
}
