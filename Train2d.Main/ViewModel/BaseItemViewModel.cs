using System.Windows;
using Train2d.Model.Converter;
using Train2d.Model.Items;

namespace Train2d.Main.ViewModel
{
  class BaseItemViewModel : ViewModelBase
  {
    #region Attributes

    private Item _item;

    #endregion

    #region Properties

    public Vector Position
    {
      get
      {
        if (_item.Position.HasValue)
        {
          return _item.Position.Value.ToVector();
        }
        return new Vector(0, 0);
      }
      private set { }
    }

    #endregion


  }
}
