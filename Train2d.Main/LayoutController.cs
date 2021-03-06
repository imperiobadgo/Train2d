﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Train2d.Main.ViewModel.Items;
using Train2d.Model;
using Train2d.Model.Items;

namespace Train2d.Main
{
  public class LayoutController
  {
    #region Attributes

    private readonly Dictionary<Guid, ItemViewModel> _content;
    private readonly Dictionary<Coordinate, List<Guid>> _layout;

    #endregion

    #region Construct

    public LayoutController()
    {
      _content = new Dictionary<Guid, ItemViewModel>();
      _layout = new Dictionary<Coordinate, List<Guid>>();
      Items = new ObservableCollection<ItemViewModel>();
    }

    public static ItemViewModel GetItemViewModel(Item newItem)
    {
      if (newItem is Track)
      {
        return new TrackViewModel((Track)newItem);
      }
      if (newItem is TrackSwitch)
      {
        return new TrackSwitchViewModel((TrackSwitch)newItem);
      }
      if (newItem is Train)
      {
        return new TrainViewModel((Train)newItem);
      }
      if (newItem is Signal)
      {
        return new SignalViewModel((Signal)newItem);
      }
      return null;
    }

    #endregion

    #region Methods - Layout

    /// <summary>
    /// Tries to add new item to the layout
    /// </summary>
    /// <returns>True if new Item could be added, False if item already exits</returns>
    public bool AddItemToLayout(Coordinate position, ItemViewModel newItem)
    {
      if (!newItem.Id.HasValue)
      {
        return false;
      }

      if (_layout.TryGetValue(position, out List<Guid> itemsAtPosition))
      {
        if (itemsAtPosition.Contains(newItem.Id.Value))
        {
          return false;
        }
        itemsAtPosition.Add(newItem.Id.Value);
        newItem.SetCoordinate(position);
      }
      else
      {
        _layout.Add(position, new List<Guid>(new Guid[] { newItem.Id.Value }));
        newItem.SetCoordinate(position);
      }
      return true;
    }

    /// <summary>
    /// Tries to remove an item from the layout
    /// </summary>
    /// <param name="itemToRemove"></param>
    /// <returns>True if new Item could be removed, False if item is not at the position</returns>
    public bool RemoveItemFromLayout(Coordinate position, ItemViewModel itemToRemove)
    {
      if (!itemToRemove.Id.HasValue)
      {
        return false;
      }
      if (_layout.TryGetValue(position, out List<Guid> itemsAtPosition))
      {
        if (itemsAtPosition.Contains(itemToRemove.Id.Value))
        {
          itemsAtPosition.Remove(itemToRemove.Id.Value);
          itemToRemove.SetCoordinate(null);
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Returns all LayoutItems at this Coordinate
    /// </summary>
    /// <returns>if no Items are found, the array contains 0 elements</returns>
    public List<ItemViewModel> GetLayoutItems(Coordinate position)
    {
      List<ItemViewModel> result = new List<ItemViewModel>();
      if (_layout.TryGetValue(position, out List<Guid> itemsAtPosition))
      {
        foreach (var id in itemsAtPosition)
        {
          ItemViewModel item = GetLayoutItemFromId(id);
          if (item != null)
          {
            result.Add(item);
          }
        }
      }
      return result;
    }

    #endregion

    #region Methods - Content

    /// <summary>
    /// Adds a LayoutItem to the guid-dictionary
    /// </summary>
    /// <returns>True when added, False if already exists</returns>
    public bool AddLayoutItem(ItemViewModel newItem)
    {
      if (newItem.Id.HasValue)
      {
        return false;
      }
      Guid newGuid = Guid.NewGuid();
      _content.Add(newGuid, newItem);
      newItem.SetGuid(newGuid);
      InsertItemTypeSorted(newItem);
      return true;
    }

    public void InsertItemTypeSorted(ItemViewModel newItem)
    {
      ItemViewModel lastItemWithSameDisplayOrder = Items.OrderBy(x => x.DisplayOrder).LastOrDefault(x => x.DisplayOrder <= newItem.DisplayOrder);
      if (lastItemWithSameDisplayOrder == null)
      {
        Items.Add(newItem);
      }
      else
      {
        int index = Items.IndexOf(lastItemWithSameDisplayOrder);
        Items.Insert(index + 1, newItem);
      }

    }

    /// <summary>
    /// Tries to remove the LayoutItem from the guid-dictionary
    /// </summary>
    /// <returns>True if iem could be removed, False if item is not in the dictionary</returns>
    public bool RemoveLayoutItem(ItemViewModel itemToRemove)
    {
      if (!itemToRemove.Id.HasValue)
      {
        return false;
      }
      if (_content.Remove(itemToRemove.Id.Value))
      {
        Items.Remove(itemToRemove);
        itemToRemove.SetGuid(null);
        return true;
      }
      return false;
    }

    /// <summary>
    /// Tries to return the corresponding LayoutItem
    /// </summary>
    public ItemViewModel GetLayoutItemFromId(Guid? id)
    {
      if (!id.HasValue)
      {
        return null;
      }
      if (_content.TryGetValue(id.Value, out ItemViewModel item))
      {
        return item;
      }
      return null;
    }

    #endregion

    #region Methods - Modelconversion

    public Layout GetLayout()
    {
      Layout result = new Layout();
      result.ContentItems = _content.Select(x => x.Value.BaseItem()).ToList();
      result.LayoutItems = _layout.Select(x => new LayoutPosition(x.Key, x.Value)).ToList();
      return result;
    }

    public void SetLayout(Layout layout)
    {
      _layout.Clear();
      _content.Clear();
      Items.Clear();
      layout.ContentItems.ForEach(x =>
      {
        ItemViewModel newItem = GetItemViewModel(x);
        _content.Add(newItem.Id.Value, newItem);
        Items.Add(newItem);
      });
      layout.LayoutItems.ForEach(x => _layout.Add(x.Position, x.ItemIds));
    }

    #endregion

    #region View

    public ObservableCollection<ItemViewModel> Items { get; private set; }

    #endregion

  }
}
