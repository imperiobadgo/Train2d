using System;
using System.Collections.Generic;
using Train2d.Model.Items;

namespace Train2d.Model
{
  public class MainLayoutController
  {
    #region Attributes

    private readonly Dictionary<Guid, Item> _content;
    private readonly Dictionary<Coordinate, List<Guid>> _layout;

    #endregion

    #region Construct

    public MainLayoutController()
    {
      _content = new Dictionary<Guid, Item>();
      _layout = new Dictionary<Coordinate, List<Guid>>();
    }

    #endregion

    #region Methods - Layout

    /// <summary>
    /// Tries to a new item to the layout
    /// </summary>
    /// <returns>True if new Item could be added, False if item already exits</returns>
    public bool AddItemToLayout(Coordinate position, Item newItem)
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
        newItem.SetPosition(position);
      }
      else
      {
        _layout.Add(position, new List<Guid>(new Guid[] { newItem.Id.Value }));
      }
      return true;
    }

    /// <summary>
    /// Tries to remove an item from the layout
    /// </summary>
    /// <param name="itemToRemove"></param>
    /// <returns>True if new Item could be removed, False if item is not at the position</returns>
    public bool RemoveItemFromLayout(Coordinate position, Item itemToRemove)
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
          itemToRemove.SetPosition(null);
          return true;
        }
      }
      return false;
    }

    /// <summary>
    /// Returns all LayoutItems at this Coordinate
    /// </summary>
    /// <returns>if no Items are found, the array contains 0 elements</returns>
    public Item[] GetLayoutItems(Coordinate position)
    {
      List<Item> result = new List<Item>();
      if (_layout.TryGetValue(position, out List<Guid> itemsAtPosition))
      {
        foreach (var id in itemsAtPosition)
        {
          Item item = GetLayoutItemFromId(id);
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
    public bool AddLayoutItem(Item newItem)
    {
      if (newItem.Id.HasValue)
      {
        return false;
      }
      if (_content.ContainsKey(newItem.Id.Value))
      {
        return false;
      }
      Guid newGuid = Guid.NewGuid();
      _content.Add(newGuid, newItem);
      
      return true;
    }

    /// <summary>
    /// Tries to remove the LayoutItem from the guid-dictionary
    /// </summary>
    /// <returns>True if iem could be removed, False if item is not in the dictionary</returns>
    public bool RemoveLayoutItem(Item itemToRemove)
    {
      if (!itemToRemove.Id.HasValue)
      {
        return false;
      }
      if (_content.Remove(itemToRemove.Id.Value))
      {
        itemToRemove.SetGuid(null);
        return true;
      }
      return false;
    }

    /// <summary>
    /// Tries to return the corresponding LayoutItem
    /// </summary>
    public Item GetLayoutItemFromId(Guid id)
    {
      if (_content.TryGetValue(id, out Item item))
      {
        return item;
      }
      return null;
    }

    #endregion

  }
}
