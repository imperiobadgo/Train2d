using System;
using System.Windows.Data;

namespace Train2d.Main.Converter
{
  public class ScaleInverseConverter : IValueConverter
  {
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      var param = System.Convert.ToDouble(parameter);
      if (value is double)
      {
        double result = param / System.Convert.ToDouble(value);
        return result;
      }
      else
        throw new ArgumentException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
      throw new Exception("The method or operation is not implemented.");
    }
  }
}
