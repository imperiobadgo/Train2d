using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Train2d.Model;
using Train2d.Model.Converter;

namespace Train2d.Main.Controls
{
  /// <summary>
  /// Interaction logic for LayoutView.xaml
  /// </summary>
  public partial class LayoutView : UserControl
  {
    #region Attributes

    private Point _start;
    private Vector _newPosition;
    private bool _allowTranslate;
    private Coordinate _mouseCoordinate;

    #endregion

    #region Construct

    public LayoutView()
    {
      InitializeComponent();

      InitializeMouseHandler();
    }

    private void InitializeMouseHandler()
    {
      UserControl.MouseWheel += OnContentMouseWheel;
      ZoomContent.MouseDown += OnContentMouseButtonDown;
      ZoomContent.MouseUp += OnContentMouseButtonUp;
      UserControl.MouseMove += OnContentMouseMove;
      UserControl.MouseDown += OnContentMouseButtonDown;
      UserControl.MouseUp += OnContentMouseButtonUp;
    }

    private void OnContentMouseButtonDown(object sender, MouseButtonEventArgs e)
    {

      if (e.ChangedButton == UserSettings.SelectMain)
        Settings.ExecuteSelectMain();
      if (e.ChangedButton == UserSettings.SelectSub)
        Settings.ExecuteSelectSub();
      if (e.ChangedButton == UserSettings.SelectDrag)
      {
        if (e.ClickCount == 2)
        {
          ResetZoomAndScroll();
          return;
        }

        _start = e.GetPosition(LayoutGrid);

        _newPosition.X = Settings.Translate.X;
        _newPosition.Y = Settings.Translate.Y;

        // The previous mouse capture was done here, but this stopped inner MouseRightButtonUp from working
        // ZoomContent.CaptureMouse()
        _allowTranslate = true;
      }

    }

    private void OnContentMouseButtonUp(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton == UserSettings.SelectMain)
        Settings.ExecuteDeselectMain();
      if (e.ChangedButton == UserSettings.SelectSub)
        Settings.ExecuteDeselectSub();
      if (e.ChangedButton == UserSettings.SelectDrag)
      {
        ZoomContent.ReleaseMouseCapture();
        _allowTranslate = false;
      }
    }

    #endregion

    #region Methods

    private void OnContentMouseMove(object sender, MouseEventArgs e)
    {
      OnContentMouseMoveTranslate(e);
      OnContentMouseMovePosition(); // sender, e)
      Settings.ExecuteMouseMove();

      Coordinate newMouseCoordinate = MousePoint.ToCoordinate();
      if (Equals(newMouseCoordinate, _mouseCoordinate))
      {
        return;
      }
      _mouseCoordinate = newMouseCoordinate;
      Settings.MouseCoordinate = _mouseCoordinate;
      Settings.ExecuteMouseCoordinateChanged();
    }

    private void OnContentMouseMoveTranslate(MouseEventArgs e)
    {
      bool correctButtonPressed = false;
      if (UserSettings.SelectDrag == MouseButton.Left && e.LeftButton == MouseButtonState.Pressed)
        correctButtonPressed = true;
      if (UserSettings.SelectDrag == MouseButton.Right && e.RightButton == MouseButtonState.Pressed)
        correctButtonPressed = true;
      if (UserSettings.SelectDrag == MouseButton.Middle && e.MiddleButton == MouseButtonState.Pressed)
        correctButtonPressed = true;
      if (UserSettings.SelectDrag == MouseButton.XButton1 && e.XButton1 == MouseButtonState.Pressed)
        correctButtonPressed = true;
      if (UserSettings.SelectDrag == MouseButton.XButton2 && e.XButton2 == MouseButtonState.Pressed)
        correctButtonPressed = true;

      // Capture mouse here to allow clicking child elements
      if (_allowTranslate && correctButtonPressed && !ZoomContent.IsMouseCaptured)
        ZoomContent.CaptureMouse();

      if (!ZoomContent.IsMouseCaptured)
        return;

      Vector delta = _start - e.GetPosition(LayoutGrid);

      // Fix for double-click reset
      if (Math.Abs(delta.Length) < double.Epsilon)
        return;

      var offset = _newPosition - delta;
      Settings.SetOffset(offset);
    }

    private void OnContentMouseMovePosition() // sender As Object, e As MouseEventArgs)
    {
      var absoluteMousePositionOnZoomBox = Mouse.GetPosition(this);
      var zoomBoxCenter = new Point(ActualWidth / (double)2, ActualHeight / (double)2);

      var zoomBoxOrigin = Settings.Translate.Transform(zoomBoxCenter);

      var posRelativeToCenter = absoluteMousePositionOnZoomBox - zoomBoxOrigin;

      posRelativeToCenter.Y = -posRelativeToCenter.Y;
      posRelativeToCenter /= (double)Settings.ScaleFactor;

      MousePoint = new Point(posRelativeToCenter.X, posRelativeToCenter.Y);
      Settings.MousePosition = MousePoint;
    }

    private static Point GetPosition(Point zoomBoxCenter, LayoutViewSettings settings, Point mousePos)
    {
      TranslateTransform trans = settings.Translate;
      var zoomBoxOrigin = trans.Transform(zoomBoxCenter);

      var posRelativeToCenter = mousePos - zoomBoxOrigin;
      posRelativeToCenter.Y = -posRelativeToCenter.Y;
      posRelativeToCenter /= (double)settings.ScaleFactor;
      return new Point(posRelativeToCenter.X, posRelativeToCenter.Y);
    }

    private void OnContentMouseWheel(object sender, MouseWheelEventArgs e)
    {
      e.Handled = true;

      var mousePos = Mouse.GetPosition(this);
      var zoomBoxCenter = new Point(ActualWidth / (double)2, ActualHeight / (double)2);
      var p1 = GetPosition(zoomBoxCenter, Settings, mousePos);

      double zoom = e.Delta > 0 ? UserSettings.ZoomIncrements : -UserSettings.ZoomIncrements;
      double linearZoom = Settings.ScaleFactor * zoom;//Convert to linear zoom for consistent zoom change at small and big zoom factor
      Settings.SetZoom(linearZoom);

      var p2 = GetPosition(zoomBoxCenter, Settings, mousePos);
      var delta = (p2 - p1);
      delta *= Settings.ScaleFactor;

      if (Settings.ScaleToPosition)
      {
        Settings.Translate.X += delta.X;
        Settings.Translate.Y -= delta.Y;
      }
    }

    private void ResetZoomAndScroll()
    {
      Settings.ResetZoomAndScroll();
    }

    #endregion

    #region Properties

    public UIElement CanvasContent
    {
      get
      {
        return (UIElement)GetValue(CanvasContentProperty);
      }
      set
      {
        SetValue(CanvasContentProperty, value);
      }
    }

    public static readonly DependencyProperty CanvasContentProperty = DependencyProperty.Register(
      nameof(CanvasContent),
      typeof(UIElement),
      typeof(LayoutView),
      new PropertyMetadata(null));

    public bool ShowControls
    {
      get
      {
        return (bool)GetValue(ShowControlsProperty);
      }
      set
      {
        SetValue(ShowControlsProperty, value);
      }
    }

    public static readonly DependencyProperty ShowControlsProperty = DependencyProperty.Register(
      nameof(ShowControls),
      typeof(bool),
      typeof(LayoutView),
      new PropertyMetadata(false));


    public bool ShowPositionY
    {
      get
      {
        return (bool)GetValue(ShowPositionYProperty);
      }
      set
      {
        SetValue(ShowPositionYProperty, value);
      }
    }

    public static readonly DependencyProperty ShowPositionYProperty = DependencyProperty.Register(
      nameof(ShowPositionY),
      typeof(bool),
      typeof(LayoutView),
      new PropertyMetadata(true));




    public Point MousePoint
    {
      get
      {
        return (Point)GetValue(MousePointProperty);
      }
      set
      {
        SetValue(MousePointProperty, value);
      }
    }

    public static readonly DependencyProperty MousePointProperty = DependencyProperty.Register(
      nameof(MousePoint),
      typeof(Point),
      typeof(LayoutView),
      new PropertyMetadata(new Point()));



    public LayoutViewSettings Settings
    {
      get
      {
        return (LayoutViewSettings)GetValue(SettingsProperty);
      }
      set
      {
        SetValue(SettingsProperty, value);
      }
    }

    public static readonly DependencyProperty SettingsProperty = DependencyProperty.Register(
      nameof(Settings),
      typeof(LayoutViewSettings),
      typeof(LayoutView),
      new PropertyMetadata(
        new LayoutViewSettings(),
        OnSettingsChanged));

    public static void OnSettingsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      var view = sender as LayoutView;
      if (view == null)
        return;

      // Dim group As TransformGroup = view.Settings.Group
      view.ZoomContent.RenderTransform = view.Settings.Group;
      view.ShowControls = view.Settings.ShowSettings;
      view.ShowPositionY = view.Settings.ShowPositionY;
    }

    public UserSettings UserSettings
    {
      get
      {
        return (UserSettings)GetValue(UserSettingsProperty);
      }
      set
      {
        SetValue(UserSettingsProperty, value);
      }
    }

    public static readonly DependencyProperty UserSettingsProperty = DependencyProperty.Register(
      nameof(UserSettings),
      typeof(UserSettings),
      typeof(LayoutView),
      new PropertyMetadata(
        new UserSettings(),
        OnUserSettingsChanged));

    public static void OnUserSettingsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      var view = sender as LayoutView;
      if (view == null)
        return;

      //// Dim group As TransformGroup = view.Settings.Group
      //view.ZoomContent.RenderTransform = view.Settings.Group;
      //view.ShowControls = view.Settings.ShowSettings;
      //view.ShowPositionY = view.Settings.ShowPositionY;
    }

    public double VisualWidth
    {
      get
      {
        return (double)GetValue(VisualWidthProperty);
      }
      set
      {
        SetValue(VisualWidthProperty, value);
      }
    }

    public static readonly DependencyProperty VisualWidthProperty = DependencyProperty.Register(
      nameof(VisualWidth),
      typeof(double),
      typeof(LayoutView),
      new PropertyMetadata(1.0));



    public string PositionText
    {
      get
      {
        return System.Convert.ToString(GetValue(PositionTextProperty));
      }
      set
      {
        SetValue(PositionTextProperty, value);
      }
    }

    public static readonly DependencyProperty PositionTextProperty = DependencyProperty.Register(
      nameof(PositionText),
      typeof(string),
      typeof(LayoutView),
      new PropertyMetadata(string.Empty));

    #endregion
  }
}
