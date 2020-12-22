using Train2d.Main.ViewModel;
using Train2d.Main.ViewModel.Items;

namespace Train2d.Main.Commands
{
  public class CreateItemCommand : CommandItemBase<ItemViewModel>
  {

    public CreateItemCommand(LayoutViewModel viewModel, ItemViewModel newItem) : base(viewModel, newItem)
    { }

    protected override bool Execute()
    {
      return _viewModel.LayoutController.AddLayoutItem(_item);
    }

    protected override void Undo()
    {
      _viewModel.LayoutController.RemoveLayoutItem(_item);
    }
  }
}
