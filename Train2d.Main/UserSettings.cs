using System.Windows.Input;

namespace Train2d.Main
{
  public class UserSettings
  {

    public MouseButton SelectMain = MouseButton.Left;
    public MouseButton SelectSub = MouseButton.Right;
    public MouseButton SelectDrag = MouseButton.Right;
    public MouseButton ResetDrag = MouseButton.Right;
    public double ZoomIncrements = 0.2;
  }
}
