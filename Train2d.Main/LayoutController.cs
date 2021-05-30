using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Train2d.Main.ViewModel;
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
    private readonly List<IUpdateableItem> _updateableItems;
    #endregion

    #region Construct

    public LayoutController()
    {
      _content = new Dictionary<Guid, ItemViewModel>();
      _layout = new Dictionary<Coordinate, List<Guid>>();
      _updateableItems = new List<IUpdateableItem>();
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

    #region Methods - Update

    public void Update(float deltaTime)
    {
      foreach (var item in _updateableItems)
      {
        item.Update(deltaTime);
      }
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
      }
      else
      {
        _layout.Add(position, new List<Guid>(new Guid[] { newItem.Id.Value }));
      }
      newItem.SetCoordinate(position);
      if (newItem is IUpdateableItem updateableItem)
      {
        if (!_updateableItems.Contains(updateableItem))
        {
          _updateableItems.Add(updateableItem);
        }

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
          if (itemToRemove is IUpdateableItem updateableItem)
          {
            _updateableItems.Remove(updateableItem);
          }
          if (itemsAtPosition.Count == 0)
          {
            //Remove not needed empty layout position
            _layout.Remove(position);
          }
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

    public List<TrackViewModel> GetTracksOnPosition(Coordinate centerCoordinate)
    {
      List<TrackViewModel> checkTracks = new List<TrackViewModel>();
      //Searchrange needs to be 5x5 because horizontal and antidiogonal tracks won't be catched otherwise
      for (int x = centerCoordinate.X - 2; x <= centerCoordinate.X + 2; x++)
      {
        for (int y = centerCoordinate.Y - 2; y <= centerCoordinate.Y + 2; y++)
        {
          List<ItemViewModel> itemsOnPosition = GetLayoutItems(new Coordinate(x, y));
          foreach (var item in itemsOnPosition.OfType<TrackViewModel>())
          {
            if (!checkTracks.Contains(item))
            {
              checkTracks.Add(item);
            }
          }          
        }
      }
      List<TrackViewModel> result = new List<TrackViewModel>();
      foreach (var track in checkTracks)
      {
        if (track.ContainsCoordinate(centerCoordinate))
        {
          result.Add(track);
        }
      }
      return result;
    }

    public List<TrackViewModel> GetAdjacentTracksOnPosition(Coordinate position)
    {
      List<TrackViewModel> result = new List<TrackViewModel>();

      if (_layout.TryGetValue(position, out List<Guid> itemsAtPosition))
      {
        foreach (var id in itemsAtPosition)
        {
          ItemViewModel item = GetLayoutItemFromId(id);
          if (item is TrackSwitchViewModel switchTrack)
          {
            return switchTrack.GetTracks(this);
          }
          if (item is TrackViewModel track)
          {
            result.Add(track);
          }
        }
      }


      for (int x = position.X - 1; x <= position.X + 1; x++)
      {
        for (int y = position.Y - 1; y <= position.Y + 1; y++)
        {
          Coordinate checkCoordinate = new Coordinate(x, y);
          if (_layout.TryGetValue(checkCoordinate, out List<Guid> itemsAtCheckPosition))
          {
            foreach (var id in itemsAtCheckPosition)
            {
              ItemViewModel item = GetLayoutItemFromId(id);
              if (item is TrackViewModel track)
              {
                if (track.ContainsCoordinate(position))
                {
                  if (!result.Contains(track))
                  {
                    result.Add(track);
                  }
                }
              }
            }
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
    public bool AddLayoutItem(LayoutViewModel layout, ItemViewModel newItem, Guid? newGuid = null)
    {
      if (newItem.Id.HasValue)
      {
        return false;
      }
      if (newGuid == null)
      {
        newGuid = Guid.NewGuid();
      }
      _content.Add(newGuid.Value, newItem);
      newItem.SetGuid(layout, newGuid);
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
    /// <returns>True, if item could be removed, False, if item is not in the dictionary</returns>
    public bool RemoveLayoutItem(ItemViewModel itemToRemove)
    {
      if (!itemToRemove.Id.HasValue)
      {
        return false;
      }
      if (_content.Remove(itemToRemove.Id.Value))
      {
        Items.Remove(itemToRemove);
        itemToRemove.SetGuid(null, null);
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

    public void SetLayout(LayoutViewModel viewModel, Layout layout)
    {
      _layout.Clear();
      _content.Clear();
      _updateableItems.Clear();
      Items.Clear();      
      layout.ContentItems.ForEach(x =>
      {
        ItemViewModel newItem = GetItemViewModel(x);
        _content.Add(newItem.Id.Value, newItem);
        if (newItem is IUpdateableItem updateableItem && newItem.Coordinate.HasValue)
        {
          _updateableItems.Add(updateableItem);
        }
        newItem.SetLayout(viewModel);
        InsertItemTypeSorted(newItem);
      });
      layout.LayoutItems.ForEach(x => _layout.Add(x.Position, x.ItemIds));
    }

    #endregion

    #region View

    public ObservableCollection<ItemViewModel> Items { get; private set; }

    #endregion

  }
}
