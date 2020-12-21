using Train2d.Model.Items;

namespace Train2d.Model.Commands
{
  public class DeleteItemCommand : CommandItemBase
  {

    public DeleteItemCommand(MainLayoutController controller, Item newItem) : base(controller, newItem)
    { }

    protected override bool Execute()
    {
      return _controller.RemoveLayoutItem(_item);
    }

    protected override void Undo()
    {
      _controller.AddLayoutItem(_item);
    }
  }
}
