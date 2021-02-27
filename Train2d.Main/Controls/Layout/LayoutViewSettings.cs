using System;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using Train2d.Model;
using Train2d.Model.Converter;

namespace Train2d.Main.Controls
{
  public class LayoutViewSettings : INotifyPropertyChanged
  {
    #region Attributes

    private double _scaleFactor = 1.0;
    private Action _onSelectMainAction;
    private Action _onDeselectMainAction;
    private Action _onSelectSubAction;
    private Action _onDeselectSubAction;
    private Action _onMouseMoveAction;
    private Action _onMouseCoordinateChangedAction;
    private Coordinate _mouseCoordinate;

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

    public LayoutViewSettings(Action onSelectMainAction) : this()
    {
      _onSelectMainAction = onSelectMainAction;
    }

    public void SetOnSelectMainAction(Action onSelectMainAction) => _onSelectMainAction = onSelectMainAction;

    public void SetOnDeselectMainAction(Action onDeselectMainAction) => _onDeselectMainAction = onDeselectMainAction;

    public void SetOnSelectSubAction(Action onSelectSubAction) => _onSelectSubAction = onSelectSubAction;

    public void SetOnDeselectSubAction(Action onDeselectSubAction) => _onDeselectSubAction = onDeselectSubAction;

    public void SetOnMouseMoveAction(Action onMouseMoveAction) => _onMouseMoveAction = onMouseMoveAction;

    public void SetOnMouseCoordinateChangedAction(Action onMouseCoordinateChangedAction) => _onMouseCoordinateChangedAction = onMouseCoordinateChangedAction;

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

    public void ExecuteSelectMain() => _onSelectMainAction?.Invoke();

    public void ExecuteDeselectMain() => _onDeselectMainAction?.Invoke();

    public void ExecuteSelectSub() => _onSelectSubAction?.Invoke();

    public void ExecuteDeselectSub() => _onDeselectSubAction?.Invoke();

    public void ExecuteMouseMove() => _onMouseMoveAction?.Invoke();

    public void ExecuteMouseCoordinateChanged() => _onMouseCoordinateChangedAction?.Invoke();

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

    public Vector MouseCoordinatePosition { get => MouseCoordinate.ToVector(); set { } }

    public Coordinate MouseCoordinate
    {
      get => _mouseCoordinate; 
      set
      {
        _mouseCoordinate = value;
        NotifyPropertyChanged(nameof(MouseCoordinatePosition));
      }
    }
    #endregion

    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
    {
      NotifyPropertyChanged(propertyName);
    }

    protected void NotifyPropertyChanged(string info)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
    }
  }
}

