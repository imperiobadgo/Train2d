using System.Windows;


namespace Train2d.Main.Controls.Layout
{
  /// <summary>
  /// Interaction logic for LayoutCoordinates.xaml
  /// </summary>
  public partial class LayoutCoordinates
  {
    public LayoutCoordinates()
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
      typeof(LayoutCoordinates),
      new PropertyMetadata(1.0));

    #endregion

    //Get
    //    Return GetValue(ScaleFactorProperty)
    //  End Get
    //  Set
    //    SetValue(ScaleFactorProperty, Value)
    //  End Set
    //End Property

    //Public Shared ReadOnly ScaleFactorProperty As DependencyProperty = DependencyProperty.Register(NameOf(ScaleFactor),
    //                                                                                               GetType(Double),
    //                                                                                               GetType(CoordinatesView),
    //                                                                                               New PropertyMetadata(1.0))



  }
}
