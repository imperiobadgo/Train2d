using System.Windows;
using System.Windows.Media;

namespace Train2d.Main.Controls.Layout
{
  /// <summary>
  /// Interaction logic for PositionIndicator.xaml
  /// </summary>
  public partial class PositionIndicator
  {
    public PositionIndicator()
    {
      InitializeComponent();
    }

    #region ScaleFactor Property

    public double ScaleFactor
    {
      get
      {
        return (double)GetValue(ScaleFactorProperty);
      }
      set
      {
        SetValue(ScaleFactorProperty, value);
      }
    }

    public static readonly DependencyProperty ScaleFactorProperty = DependencyProperty.Register(
      nameof(ScaleFactor),
      typeof(double),
      typeof(PositionIndicator),
      new PropertyMetadata(1.0));

    #endregion

    #region Position Property

    public Vector Position
    {
      get
      {
        return (Vector)GetValue(PositionProperty);
      }
      set
      {
        SetValue(PositionProperty, value);
      }
    }

    public static readonly DependencyProperty PositionProperty = DependencyProperty.Register(
      nameof(Position),
      typeof(Vector),
      typeof(PositionIndicator),
      new PropertyMetadata(new Vector(0,0)));

    #endregion

    #region Color

    public Brush Color
    {
      get
      {
        return (Brush)GetValue(ColorProperty);
      }
      set
      {
        SetValue(ColorProperty, value);
      }
    }

    public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
      nameof(Color),
      typeof(Brush),
      typeof(PositionIndicator),
      new PropertyMetadata(Brushes.Gray));

    #endregion

  }
}
