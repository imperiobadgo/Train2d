using System;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Train2d.Main.Controls
{
  public class LayoutViewSettings : INotifyPropertyChanged
  {
    #region Attributes

    private double _scaleFactor = 1.0;
    private Action _onMouseLeftButtonDownAction;
    private Action _onMouseLeftButtonUpAction;

    #endregion

    #region Construct

    public LayoutViewSettings()
    {
      Scale = new ScaleTransform();
      Translate = new TranslateTransform();

      Group = new TransformGroup();
      Group.Children.Add(Scale);
      Group.Children.Add(Translate);
    }

    public LayoutViewSettings(Action onMouseLeftButtonDownAction) : this()
    {
      _onMouseLeftButtonDownAction = onMouseLeftButtonDownAction;
    }

    public void SetOnMouseLeftButtonDownAction(Action onMouseLeftButtonDownAction)
    {
      _onMouseLeftButtonDownAction = onMouseLeftButtonDownAction;
    }

    public void SetOnMouseLeftButtonUpAction(Action onMouseLeftButtonUpAction)
    {
      _onMouseLeftButtonUpAction = onMouseLeftButtonUpAction;
    }

    #endregion

    #region Methods
    internal void ResetZoomAndScroll()
    {
      Translate.X = 0;
      Translate.Y = 0;
      Scale.ScaleX = 1;
      Scale.ScaleY = 1;
      ScaleFactor = 1;
    }

    internal void SetZoom(double relativeZoom)
    {

      // Nicht herauszoomen
      if ((Scale.ScaleX + relativeZoom) < MinimumScaleFactor | (Scale.ScaleY + relativeZoom) < MinimumScaleFactor)
      {
        Scale.ScaleX = MinimumScaleFactor;
        Scale.ScaleY = MinimumScaleFactor;
        ScaleFactor = MinimumScaleFactor;
        return;
      }

      Scale.ScaleX += relativeZoom;
      Scale.ScaleY += relativeZoom;
      ScaleFactor = Scale.ScaleX;
    }

    internal void SetOffset(Vector offset)
    {
      Translate.X = offset.X;
      Translate.Y = offset.Y;
    }

    public void ExecuteMouseLeftButtonDown()
    {
      _onMouseLeftButtonDownAction?.Invoke();
    }

    public void ExecuteMouseLeftButtonUp()
    {
      _onMouseLeftButtonUpAction?.Invoke();
    }

    #endregion

    #region Properties

    public ScaleTransform Scale { get; set; }
    public TranslateTransform Translate { get; set; }

    public TransformGroup Group { get; }

    public double ScaleFactor
    {
      get
      {
        return _scaleFactor;
      }
      set
      {
        if (!Object.Equals(_scaleFactor, value))
        {
          _scaleFactor = value;
          RaisePropertyChanged();
        }
      }
    }

    public double MinimumScaleFactor { get; set; } = 0.125;

    public bool ShowSettings { get; set; } = false;

    public bool ShowPositionY { get; set; } = true;

    public bool ScaleToPosition { get; set; } = false;

    public Point MousePosition { get; set; }

    #endregion

    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}

