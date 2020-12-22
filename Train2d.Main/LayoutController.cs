using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Train2d.Main.ViewModel;
using Train2d.Model;

namespace Train2d.Main
{
  public class LayoutController
  {
    #region Attributes

    private readonly Dictionary<Guid, BaseItemViewModel> _content;
    private readonly Dictionary<Coordinate, List<Guid>> _layout;

    #endregion

    #region Construct

    public LayoutController()
    {
      _content = new Dictionary<Guid, BaseItemViewModel>();
      _layout = new Dictionary<Coordinate, List<Guid>>();
      Items = new ObservableCollection<BaseItemViewModel>();
    }

    #endregion

    #region Methods - Layout

    /// <summary>
    /// Tries to a new item to the layout
    /// </summary>
    /// <returns>True if new Item could be added, False if item already exits</returns>
    public bool AddItemToLayout(Coordinate position, BaseItemViewModel newItem)
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
    public bool RemoveItemFromLayout(Coordinate position, BaseItemViewModel itemToRemove)
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
    public BaseItemViewModel[] GetLayoutItems(Coordinate position)
    {
      List<BaseItemViewModel> result = new List<BaseItemViewModel>();
      if (_layout.TryGetValue(position, out List<Guid> itemsAtPosition))
      {
        foreach (var id in itemsAtPosition)
        {
          BaseItemViewModel item = GetLayoutItemFromId(id);
          if (item != null)
          {
            result.Add(item);
          }
        }
      }
      return result.ToArray();
    }

    #endregion

    #region Methods - Content

    /// <summary>
    /// Adds a LayoutItem to the guid-dictionary
    /// </summary>
    /// <returns>True when added, False if already exists</returns>
    public bool AddLayoutItem(BaseItemViewModel newItem)
    {
      if (newItem.Id.HasValue)
      {
        return false;
      }
      Guid newGuid = Guid.NewGuid();      
      _content.Add(newGuid, newItem);
      newItem.SetGuid(newGuid);
      Items.Add(newItem);
      return true;
    }

    /// <summary>
    /// Tries to remove the LayoutItem from the guid-dictionary
    /// </summary>
    /// <returns>True if iem could be removed, False if item is not in the dictionary</returns>
    public bool RemoveLayoutItem(BaseItemViewModel itemToRemove)
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
    public BaseItemViewModel GetLayoutItemFromId(Guid id)
    {
      if (_content.TryGetValue(id, out BaseItemViewModel item))
      {
        return item;
      }
      return null;
    }

    #endregion

    #region View

    public ObservableCollection<BaseItemViewModel> Items { get; private set; }

    #endregion

  }
}
