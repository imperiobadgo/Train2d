using System;
using System.Diagnostics;

namespace Train2d.Model
{
  [DebuggerDisplay("x={X} y={Y}")]
  public struct Coordinate
  {
    public const double CELLSIZE = 20;
    public const double HALFCELLSIZE = CELLSIZE / 2;

    public int X;
    public int Y;

    public Coordinate(int x, int y)
    {
      X = x;
      Y = y;
    }

    public Coordinate(double x, double y)
    {
      X = (int)Math.Floor(x / CELLSIZE);
      Y = (int)Math.Floor(y / CELLSIZE);
    }

    public override bool Equals(object obj)
    {
      return obj is Coordinate coordinate &&
             X == coordinate.X &&
             Y == coordinate.Y;
    }

    public override int GetHashCode()
    {
      int hashCode = 1861411795;
      hashCode = hashCode * -1521134295 + X.GetHashCode();
      hashCode = hashCode * -1521134295 + Y.GetHashCode();
      return hashCode;
    }
  }
}
