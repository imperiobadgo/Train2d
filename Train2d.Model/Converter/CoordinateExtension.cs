using System.Windows;

namespace Train2d.Model.Converter
{
  public static class CoordinateExtension
  {
    public static Vector ToVector(this Coordinate coordinate)
    {
      return new Vector(coordinate.X * Coordinate.CELLSIZE, coordinate.Y * Coordinate.CELLSIZE);
    }

    public static Coordinate ToCoordinate(this Vector vector)
    {
      return new Coordinate(vector.X, vector.Y);
    }

    public static Point ToPoint(this Coordinate coordinate)
    {
      return new Point(coordinate.X * Coordinate.CELLSIZE, coordinate.Y * Coordinate.CELLSIZE);
    }

    public static Coordinate ToCoordinate(this Point point, double xOffset = 0, double yOffset = 0, double factor = 1)
    {
      return new Coordinate((point.X + xOffset) * factor, (point.Y + yOffset) * factor);
    }

  }
}
