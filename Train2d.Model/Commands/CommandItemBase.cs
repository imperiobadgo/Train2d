using Train2d.Model.Items;

namespace Train2d.Model.Commands
{
  public abstract class CommandItemBase : CommandBase
  {
    protected readonly MainLayoutController _controller;
    protected readonly Item _item;

    public CommandItemBase(MainLayoutController controller, Item newItem)
    {
      _controller = controller;
      _item = newItem;
    }

  }
}
