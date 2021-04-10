using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train2d.Model.Items
{
  public class Train : Item
  {

    public int Direction { get; set; } = 0;

    public int Speed { get; set; } = 0;


    #region Direction

    public const int DirectionRange = 8;

    

    #endregion

  }
}
