using System;

namespace Train2d.Model
{
  public struct Coordinate
  {
    public const int CELLSIZE = 20;

    public int X;
    public int Y;

    public Coordinate(int x, int y)
    {
      X = x;
      Y = y;
    }

    public Coordinate(double x, double y)
    {
      X = (int)Math.Round(x / CELLSIZE);
      Y = (int)Math.Round(y / CELLSIZE);
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
