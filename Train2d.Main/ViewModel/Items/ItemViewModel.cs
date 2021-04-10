using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Train2d.Model;
using Train2d.Model.Converter;
using Train2d.Model.Items;

namespace Train2d.Main.ViewModel.Items
{
  public abstract class ItemViewModel : ViewModelBase
  {
    #region Attributes

    private Item _item;
    private Brush _mainColor;

    #endregion

    #region Construct

    protected ItemViewModel()
    { }

    protected void SetItem(Item item)
    {
      _item = item;
      OnItemSet(item);
    }

    protected abstract void OnItemSet(Item item);

    public Item BaseItem() => _item;

    #endregion

    #region Methods

    public void SetGuid(Guid? newId)
    {
      _item.Id = newId;
    }

    public void SetCoordinate(Coordinate? _position)
    {
      _item.Coordinate = _position;
      NotifyPropertyChanged(nameof(Position));
    }

    public virtual void OnSelectMain(LayoutController layoutController)
    {

    }

    public List<Tuple<int, Coordinate>> GetCoordinatesInDirection(int direction)
    {
      List<Tuple<int, Coordinate>> coordinates = new List<Tuple<int, Coordinate>>();
      List<int> possibleDirections = new List<int>();
      for (int i = direction - 1; i < direction + 2; i++)
      {
        if (i < 0)
        {
          possibleDirections.Add(i + Train.DirectionRange);
        }
        else if (i > Train.DirectionRange - 1)
        {
          possibleDirections.Add(i - Train.DirectionRange);
        }
        else
        {
          possibleDirections.Add(i);
        }
      }
      foreach (int dir in possibleDirections)
      {
        Coordinate? possibleCoord = _item.GetCoordinateInDirection(dir);
        if (possibleCoord.HasValue)
        {
          coordinates.Add(Tuple.Create(dir, possibleCoord.Value));
        }
      }

      return coordinates;
    }


    #endregion

    #region Properties

    public Guid? Id { get => _item.Id; private set { _item.Id = value; } }
    public Coordinate? Coordinate { get => _item.Coordinate; private set { _item.Coordinate = value; } }

    public Vector Position
    {
      get
      {
        if (Coordinate.HasValue)
        {
          return Coordinate.Value.ToVector();
        }
        return new Vector(0, 0);
      }
      private set { }
    }

    /// <summary>
    /// Larger values means rendered on top
    /// </summary>
    public int DisplayOrder { get; protected set; } = 0;

    public Brush MainColor
    {
      get => _mainColor;
      protected set
      {
        _mainColor = value;
        DisplayedColor = value;
      }
    }

    public Brush DisplayedColor { get; protected set; }

    #endregion

  }
}
