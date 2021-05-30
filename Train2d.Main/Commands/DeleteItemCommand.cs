using System;
using Train2d.Main.ViewModel;
using Train2d.Main.ViewModel.Items;

namespace Train2d.Main.Commands
{
  public class DeleteItemCommand : CommandItemBase<ItemViewModel>
  {
    Guid? _oldGuid;
    public DeleteItemCommand(LayoutViewModel viewModel, ItemViewModel newItem) : base(viewModel, newItem)
    { }

    protected override bool Execute()
    {
      _oldGuid = _item.Id;
      return _viewModel.LayoutController.RemoveLayoutItem(_item);
    }

    protected override void Undo()
    {
      _viewModel.LayoutController.AddLayoutItem(_viewModel, _item, _oldGuid);
    }

    public override string ToString()
    {
      return $"Deleted {_item}";
    }
  }
}
