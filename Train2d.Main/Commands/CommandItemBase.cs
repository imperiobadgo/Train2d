using Train2d.Main.ViewModel;

namespace Train2d.Main.Commands
{
  public abstract class CommandItemBase : CommandBase
  {
    protected readonly LayoutViewModel _viewModel;
    protected readonly BaseItemViewModel _item;

    public CommandItemBase(LayoutViewModel viewModel, BaseItemViewModel newItem)
    {
      _viewModel = viewModel;
      _item = newItem;
    }

  }
}
