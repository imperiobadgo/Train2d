using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train2d.Model.Items
{
  public class TrackSwitch : Item
  {
    public Guid? Track { get; set; }

    public List<Guid> AdjacentTracks { get; set; } = new List<Guid>();
  }
}
