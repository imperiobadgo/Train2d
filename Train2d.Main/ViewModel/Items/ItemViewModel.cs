﻿using System;
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
    {}

    protected void SetItem(Item item) => _item = item;

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
