using Train2d.Model.Items;

namespace Train2d.Model.Commands
{
  class PositionItemCommand : CommandItemBase
  {
    private Coordinate _coordinate;

    public PositionItemCommand(MainLayoutController controller, Item item, Coordinate coordinate) : base(controller, item)
    {
      _coordinate = coordinate;
    }

    protected override bool Execute()
    {
      return _controller.AddItemToLayout(_coordinate, _item);
    }

    protected override void Undo()
    {
      _controller.RemoveItemFromLayout(_coordinate, _item);
    }
  }
}
