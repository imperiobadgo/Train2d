using Train2d.Model.Items;

namespace Train2d.Model.Commands
{
  public class CreateItemCommand : CommandItemBase
  {

    public CreateItemCommand(MainLayoutController controller, Item newItem) : base(controller, newItem)
    { }

    protected override bool Execute()
    {
      return _controller.AddLayoutItem(_item);
    }

    protected override void Undo()
    {
      _controller.RemoveLayoutItem(_item);
    }
  }
}
