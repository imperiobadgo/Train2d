using System;
using Train2d.Main.ViewModel;
using Train2d.Main.ViewModel.Items;

namespace Train2d.Main.Commands
{
  public class CreateItemCommand : CommandItemBase<ItemViewModel>
  {
    private Guid _newGuid;
    public CreateItemCommand(LayoutViewModel viewModel, ItemViewModel newItem, Guid newGuid) : base(viewModel, newItem)
    {
      _newGuid = newGuid;
    }

    protected override bool Execute()
    {
      return _viewModel.LayoutController.AddLayoutItem(_item, _newGuid);
    }

    protected override void Undo()
    {
      _viewModel.LayoutController.RemoveLayoutItem(_item);
    }

    public override string ToString()
    {
      return $"Created {_item}";
    }

  }
}
