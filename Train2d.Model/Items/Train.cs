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

    /// <summary>
    /// 7  0  1
    /// 6     2
    /// 5  4  3
    /// </summary>
    /// <returns></returns>
    public Coordinate? GetCoordinateInDirection(int inputDirection)
    {
      if (!Coordinate.HasValue)
      {
        return null;
      }
      Coordinate coord = Coordinate.Value;
      switch (inputDirection)
      {
        case 0:
          return new Coordinate(coord.X, coord.Y + 1);
        case 1:
          return new Coordinate(coord.X + 1, coord.Y + 1);
        case 2:
          return new Coordinate(coord.X + 1, coord.Y);
        case 3:
          return new Coordinate(coord.X + 1, coord.Y - 1);
        case 4:
          return new Coordinate(coord.X, coord.Y - 1);
        case 5:
          return new Coordinate(coord.X - 1, coord.Y - 1);
        case 6:
          return new Coordinate(coord.X - 1, coord.Y);
        case 7:
          return new Coordinate(coord.X - 1, coord.Y + 1);
        default:
          break;
      }
      return null;
    }

    #endregion

  }
}
