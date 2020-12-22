using Train2d.Main.ViewModel;
using Train2d.Main.ViewModel.Items;

namespace Train2d.Main.Commands
{
  public abstract class CommandItemBase<T> : CommandBase where T : ItemViewModel
  {
    protected readonly LayoutViewModel _viewModel;
    protected readonly T _item;

    public CommandItemBase(LayoutViewModel viewModel, T newItem)
    {
      _viewModel = viewModel;
      _item = newItem;
    }

  }
}
