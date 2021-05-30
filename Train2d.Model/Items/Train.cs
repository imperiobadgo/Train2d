using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train2d.Model.Items
{
  public class Train : Item
  {

    public string TrainName { get; set; } = "Train";

    public bool ForceStop { get; set; } = false;

    public int Direction { get; set; } = 0;

    public int Speed { get; set; } = 20;

  }
}
