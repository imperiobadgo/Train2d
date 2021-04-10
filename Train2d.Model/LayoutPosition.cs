using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Train2d.Model
{
  [DebuggerDisplay("ItemsCount {ItemIds.Count} x={Position.X} y={Position.Y} ")]
  public class LayoutPosition
  {
    public LayoutPosition() { }

    public LayoutPosition(Coordinate position, List<Guid> ids)
    {
      Position = position;
      ItemIds = ids;
    }

    public Coordinate Position { get; set; }
    public List<Guid> ItemIds { get; set; }
  }
}
