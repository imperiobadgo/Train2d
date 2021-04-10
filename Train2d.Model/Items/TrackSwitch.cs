using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train2d.Model.Items
{
  public class TrackSwitch : Item
  {
    public Guid? TrackId { get; set; }

    public List<Guid> AdjacentTrackIds { get; set; } = new List<Guid>();
  }
}
