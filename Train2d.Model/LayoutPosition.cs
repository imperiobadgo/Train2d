using System;
using System.Collections.Generic;

namespace Train2d.Model
{
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
