using Train2d.Main.ViewModel;

namespace Train2d.Main.Commands
{
  public class DeleteItemCommand : CommandItemBase
  {

    public DeleteItemCommand(LayoutViewModel viewModel, BaseItemViewModel newItem) : base(viewModel, newItem)
    { }

    protected override bool Execute()
    {
      return _viewModel.LayoutController.RemoveLayoutItem(_item);
    }

    protected override void Undo()
    {
      _viewModel.LayoutController.AddLayoutItem(_item);
    }
  }
}
