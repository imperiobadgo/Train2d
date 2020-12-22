using System.Windows;

namespace Train2d.Model.Converter
{
  public static class CoordinateExtension
  {
    public static Vector ToVector(this Coordinate coordinate)
    {
      return new Vector(coordinate.X, coordinate.Y);
    }

    public static Coordinate ToCoordinate(this Vector vector)
    {
      return new Coordinate(vector.X, vector.Y);
    }

    public static Point ToPoint(this Coordinate coordinate)
    {
      return new Point(coordinate.X, coordinate.Y);
    }

    public static Coordinate ToCoordinate(this Point point)
    {
      return new Coordinate(point.X, point.Y);
    }

  }
}
