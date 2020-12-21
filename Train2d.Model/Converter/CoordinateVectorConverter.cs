using System.Windows;

namespace Train2d.Model.Converter
{
  public static class CoordinateVectorConverter
  {
    public static Vector ToVector(this Coordinate coordinate)
    {
      return new Vector(coordinate.X, coordinate.Y);
    }

    public static Coordinate ToCoordinate(this Vector vector)
    {
      return new Coordinate(vector.X, vector.Y);
    }

  }
}
