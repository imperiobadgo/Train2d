using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using Train2d.Main.Commands;
using Train2d.Main.Extensions;
using Train2d.Main.ViewModel.Items;
using Train2d.Model;
using Train2d.Model.Items;

namespace Train2d.Main.ViewModel
{
    public class EditorViewModel : ViewModelBase
  {
    #region Attributes

    private readonly LayoutViewModel _parent;
    private bool _editTracks;
    private bool _placeTrain;
    private bool _editSignals;
    private Coordinate _mouseCoordinate;
    private bool _validPosition;
    private Brush _curserColor;
    private TrackViewModel _previewTrack;
    private List<ItemViewModel> _itemsOnMousePosition = new List<ItemViewModel>();
    private ObservableCollection<ItemViewModel> _selectedItems;

    #endregion

    #region Construct

    public EditorViewModel(LayoutViewModel parent)
    {
      _parent = parent;
      SelectedItems = new ObservableCollection<ItemViewModel>();
      _parent.Settings.SetOnSelectMainAction(OnSelectMain);
      _parent.Settings.SetOnSelectSubAction(OnSelectSub);
      _parent.Settings.SetOnMouseMoveAction(OnMouseMove);
      _previewTrack = new TrackViewModel();
    }

    #endregion

    #region Methods

    private void OnSelectMain()
    {
      if (!_validPosition)
      {
        return;
      }

      List<CommandBase> commands = new List<CommandBase>();
      TrackViewModel addedTrack = null;
      if (EditTracks)
      {
        addedTrack = new TrackViewModel();
        TrackOrientation orientation = GetSelectedTrackOrientation();
        commands.Add(new CreateItemCommand(_parent, addedTrack, Guid.NewGuid()));
        commands.Add(new PositionItemOnLayoutCommand(_parent, addedTrack, _mouseCoordinate));
        commands.Add(new OrientateTrackCommand(_parent, addedTrack, orientation));
      }
      else if (PlaceTrain)
      {
        var tracksAtCoordinate = _itemsOnMousePosition.OfType<TrackViewModel>().FirstOrDefault();

        TrainViewModel newTrain = new TrainViewModel();
        commands.Add(new CreateItemCommand(_parent, newTrain, Guid.NewGuid()));
        commands.Add(new PositionItemOnLayoutCommand(_parent, newTrain, _mouseCoordinate));
        if (tracksAtCoordinate != null)
        {
          int directionToSet = 0;
          switch (tracksAtCoordinate.Orientation)
          {
            case TrackOrientation.Horizontal:
              directionToSet = 2;
              break;
            case TrackOrientation.Vertical:
              directionToSet = 0;
              break;
            case TrackOrientation.Diagonal:
              directionToSet = 1;
              break;
            case TrackOrientation.AntiDiagonal:
              directionToSet = 7;
              break;
            default:
              break;
          }
          commands.Add(new SetTrainDirectionCommand(_parent, newTrain, directionToSet));
        }
      }
      else if (EditSignals)
      {
        var signalAtCoordinate = _itemsOnMousePosition.OfType<SignalViewModel>().FirstOrDefault();
        if (signalAtCoordinate != null)
        {
          int invertedDirection = Item.InvertDirection(signalAtCoordinate.Direction);
          commands.Add(new SetSignalDirectionCommand(_parent, signalAtCoordinate, invertedDirection));
        }
        else
        {
          var trackAtCoordinate = _itemsOnMousePosition.OfType<TrackViewModel>().FirstOrDefault();
          if (trackAtCoordinate != null)
          {
            SignalViewModel newSignal = new SignalViewModel();
            commands.Add(new CreateItemCommand(_parent, newSignal, Guid.NewGuid()));
            commands.Add(new PositionItemOnLayoutCommand(_parent, newSignal, _mouseCoordinate));
            commands.Add(new SetSignalDirectionCommand(_parent, newSignal, trackAtCoordinate.GetDirectionInA()));
          }

        }

      }
      else
      {
        if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
        {
          foreach (var item in _itemsOnMousePosition)
          {
            item.OnSelectMain(_parent.LayoutController, _parent);
          }
          _parent.GetCommandController().ExecuteNewCommands();
        }
        else
        {
          SelectedItems.Clear();
          foreach (var item in _itemsOnMousePosition)
          {
            SelectedItems.Add(item);
          }
        }

      }

      if (commands.Count > 0)
      {
        CommandChain commandChain = new CommandChain(commands);
        _parent.GetCommandController().AddCommandAndExecute(commandChain);

        if (EditTracks)
        {
          List<CommandBase> switchCommands = new List<CommandBase>();
          UpdateSwitchesOnsurroundingTracks(_mouseCoordinate, switchCommands);
          foreach (CommandBase switchCommand in switchCommands)
          {
            switchCommand.ExecuteAction();
            //Add to already executed commandchain, to be part of one undo action
            commandChain.Add(switchCommand);
          }
        }
      }
      UpdateValidPosition();
    }

    private void OnSelectSub()
    {
      List<ItemViewModel> itemsToRemove = new List<ItemViewModel>();
      if (EditTracks)
      {
        itemsToRemove.AddRange(_itemsOnMousePosition.OfType<TrackViewModel>().Where(x => Equals(x.Orientation, GetSelectedTrackOrientation())));
        foreach (var trackToRemove in itemsToRemove.OfType<TrackViewModel>().ToList())
        {
          Coordinate? otherCoordinate = trackToRemove.GetOtherCoordinate(_mouseCoordinate);
          if (otherCoordinate.HasValue)
          {
            List<ItemViewModel> adjacentItemsToRemove = _parent.LayoutController.GetLayoutItems(otherCoordinate.Value);
            itemsToRemove.AddRange(adjacentItemsToRemove.OfType<SignalViewModel>());
          }
        }
      }
      else if (PlaceTrain)
      {

      }
      else if (EditSignals)
      {
        itemsToRemove.AddRange(_itemsOnMousePosition.OfType<SignalViewModel>());
      }

      List<CommandBase> removeCommands = new List<CommandBase>();
      foreach (var item in itemsToRemove)
      {
        removeCommands.Add(new RemoveItemFromLayoutCommand(_parent, item));
        removeCommands.Add(new DeleteItemCommand(_parent, item));

      }
      if (removeCommands.Count > 0)
      {
        CommandChain removeCommandsChain = new CommandChain(removeCommands);
        _parent.GetCommandController().AddCommandAndExecute(removeCommandsChain);

        List<CommandBase> switchCommands = new List<CommandBase>();
        UpdateSwitchesOnsurroundingTracks(_mouseCoordinate, switchCommands);
        foreach (CommandBase switchCommand in switchCommands)
        {
          switchCommand.ExecuteAction();
          //Add to already executed commandchain, to be part of one undo action
          removeCommandsChain.Add(switchCommand);
        }
      }
      UpdateValidPosition();
    }

    private void OnMouseMove()
    {
      Coordinate newMouseCoordinate = _parent.Settings.MousePosition.ToCoordinate();
      if (Equals(newMouseCoordinate, _mouseCoordinate))
      {
        return;
      }
      _mouseCoordinate = newMouseCoordinate;
      UpdateValidPosition();
    }

    private void UpdateValidPosition()
    {
      _itemsOnMousePosition = _parent.LayoutController.GetLayoutItems(_mouseCoordinate);
      if (EditTracks)
      {
        _validPosition = !_itemsOnMousePosition.OfType<TrackViewModel>().Any(x => Equals(x.Orientation, GetSelectedTrackOrientation()));
        UpdatePreviewTrack(_validPosition);
      }
      else if (PlaceTrain)
      {
        _validPosition = _itemsOnMousePosition.OfType<TrackViewModel>().Any();
        UpdateCurser(_validPosition);
      }
      else if (EditSignals)
      {
        var tracksOnPosition = _parent.LayoutController.GetTracksOnPosition(_mouseCoordinate);
        if (tracksOnPosition.Count > 1)
        {
          var firstTrack = tracksOnPosition.First();
          _validPosition = tracksOnPosition.All(x => x.GetDirectionInA() == firstTrack.GetDirectionInA());
        }
        else
        {
          _validPosition = false;
        }
        UpdateCurser(_validPosition);
      }
      else
      {
        _validPosition = true;
      }
      NotifyPropertyChanged(nameof(ValidPosition));
    }

    private void UpdateCurser(bool valid)
    {
      if (valid)
      {
        CurserColor = Brushes.Gray;
      }
      else
      {
        CurserColor = Brushes.Red;
      }
    }

    #endregion

    #region Update Trackswitches

    /// <summary>
    /// Check at all possible tracks around the changed position, whether switches need to be updated.
    /// </summary>
    /// <param name="switchCommands">All commands for creating, updating and deleting switches.</param>
    private void UpdateSwitchesOnsurroundingTracks(Coordinate centerCoordinate, List<CommandBase> switchCommands)
    {
      List<TrackViewModel> checkTracks = new List<TrackViewModel>();
      List<TrackSwitchViewModel> alreadyExistingSwitches = new List<TrackSwitchViewModel>();
      //Searchrange needs to be 5x5 because horizontal and antidiogonal tracks won't be catched otherwise
      for (int x = centerCoordinate.X - 2; x <= centerCoordinate.X + 2; x++)
      {
        for (int y = centerCoordinate.Y - 2; y <= centerCoordinate.Y + 2; y++)
        {
          List<ItemViewModel> itemsOnPosition = _parent.LayoutController.GetLayoutItems(new Coordinate(x, y));
          checkTracks.AddRange(itemsOnPosition.OfType<TrackViewModel>());
          alreadyExistingSwitches.AddRange(itemsOnPosition.OfType<TrackSwitchViewModel>());
        }
      }
      foreach (TrackSwitchViewModel trackSwitch in alreadyExistingSwitches)
      {
        if (!trackSwitch.TrackId.HasValue)
        {
          continue;
        }
        TrackViewModel trackWithSwitch = (TrackViewModel)_parent.LayoutController.GetLayoutItemFromId(trackSwitch.TrackId);
        if (!checkTracks.Contains(trackWithSwitch))
        {
          checkTracks.Add(trackWithSwitch);
        }
      }
      foreach (TrackViewModel checkTrack in checkTracks)
      {
        UpdateSwitchIfNecessary(checkTrack, switchCommands);
      }
    }

    /// <summary>
    /// Check around the track, whether a switch should be added, updated or removed.
    /// </summary>
    /// <param name="switchCommands">All commands for creating, updating and deleting switches.</param>
    private void UpdateSwitchIfNecessary(TrackViewModel checkTrack, List<CommandBase> switchCommands)
    {
      if (checkTrack == null)
      {
        return;
      }
      if (!checkTrack.Coordinate.HasValue || !checkTrack.EndCoordinate.HasValue)
      {
        return;
      }
      List<TrackViewModel> adjacentTracksA = new List<TrackViewModel>();
      List<TrackViewModel> adjacentTracksB = new List<TrackViewModel>();
      int addedX = checkTrack.Coordinate.Value.X;
      int addedY = checkTrack.Coordinate.Value.Y;

      //Searchrange needs to be 5x5 because horizontal and antidiogonal tracks won't be catched otherwise
      for (int x = addedX - 2; x <= addedX + 2; x++)
      {
        for (int y = addedY - 2; y <= addedY + 2; y++)
        {
          var coordinatesInA = ItemViewModel.GetCoordinatesInDirection(checkTrack.Coordinate.Value, checkTrack.GetDirectionInA());
          var coordinatesInB = ItemViewModel.GetCoordinatesInDirection(checkTrack.EndCoordinate.Value, checkTrack.GetDirectionInB());
          IEnumerable<TrackViewModel> tracks = _parent.LayoutController.GetLayoutItems(new Coordinate(x, y)).OfType<TrackViewModel>().ToList();
          //checkTrack is not added, because it does not contains the positions of coordinatesInA or coordinatesInB
          foreach (TrackViewModel track in tracks)
          {
            if (track.ContainsCoordinate(checkTrack.Coordinate.Value))
            {
              if (coordinatesInA.Where(directionCoordinate => track.ContainsCoordinate(directionCoordinate.Item2)).Any())
              {
                adjacentTracksA.Add(track);
              }
            }
            if (track.ContainsCoordinate(checkTrack.EndCoordinate.Value))
            {
              if (coordinatesInB.Where(directionCoordinate => track.ContainsCoordinate(directionCoordinate.Item2)).Any())
              {
                adjacentTracksB.Add(track);
              }
            }
          }
        }
      }
      UpdateTrackSwitch(checkTrack, checkTrack.Coordinate.Value, adjacentTracksA, switchCommands);
      UpdateTrackSwitch(checkTrack, checkTrack.EndCoordinate.Value, adjacentTracksB, switchCommands);
    }

    /// <summary>
    ///  Checks, whether a switch should be added, updated or removed.
    /// </summary>
    /// <param name="switchCommands">All commands for creating, updating and deleting switches.</param>
    private void UpdateTrackSwitch(TrackViewModel checkTrack, Coordinate checkCoordinate, List<TrackViewModel> adjacentTracks, List<CommandBase> switchCommands)
    {
      TrackSwitchViewModel alreadyExistingSwitch = _parent.LayoutController.GetLayoutItems(checkCoordinate).OfType<TrackSwitchViewModel>().FirstOrDefault();

      if (adjacentTracks.Count < 2)
      {
        if (alreadyExistingSwitch != null)
        {
          if (Equals(checkTrack.Id, alreadyExistingSwitch.TrackId))
          {
            List<CommandBase> switchRemoveCommands = new List<CommandBase>();
            switchRemoveCommands.Add(new ConfigureTrackSwitchCommand(_parent, alreadyExistingSwitch, null, new List<Guid>()));
            switchRemoveCommands.Add(new RemoveItemFromLayoutCommand(_parent, alreadyExistingSwitch));
            switchRemoveCommands.Add(new DeleteItemCommand(_parent, alreadyExistingSwitch));
            switchCommands.Add(new CommandChain(switchRemoveCommands));
          }
        }
        return;
      }

      List<Guid> adjacentTrackIds = adjacentTracks.Select(x => x.Id.Value).ToList();

      if (alreadyExistingSwitch != null)
      {
        //Just change the trackconfiguration if necessary
        List<Guid> adjacentTrackIdsOfExistingSwitch = alreadyExistingSwitch.AdjacentTrackIds;
        if (!adjacentTrackIds.SequenceEqual(adjacentTrackIdsOfExistingSwitch))
        {
          switchCommands.Add(new ConfigureTrackSwitchCommand(_parent, alreadyExistingSwitch, checkTrack.Id, adjacentTrackIds));
        }
        return;
      }

      List<CommandBase> commands = new List<CommandBase>();
      TrackSwitchViewModel trackSwitch = new TrackSwitchViewModel();
      commands.Add(new CreateItemCommand(_parent, trackSwitch, Guid.NewGuid()));
      commands.Add(new PositionItemOnLayoutCommand(_parent, trackSwitch, checkCoordinate));
      commands.Add(new ConfigureTrackSwitchCommand(_parent, trackSwitch, checkTrack.Id, adjacentTrackIds));
      switchCommands.Add(new CommandChain(commands));
    }

    #endregion

    #region Placement Tracks

    private void UpdatePreviewTrack(bool valid)
    {
      UpdateCurser(valid);
      _previewTrack.SetCoordinate(_mouseCoordinate);
      _previewTrack.SetOrientation(GetSelectedTrackOrientation());
    }

    private TrackOrientation GetSelectedTrackOrientation()
    {
      TrackOrientation orientation;
      if (Vertical)
      {
        orientation = TrackOrientation.Vertical;
      }
      else if (Diagonal)
      {
        orientation = TrackOrientation.Diagonal;
      }
      else if (AntiDiagonal)
      {
        orientation = TrackOrientation.AntiDiagonal;
      }
      else
      {
        //Backup case
        orientation = TrackOrientation.Horizontal;
      }
      return orientation;
    }

    public bool EditTracks
    {
      get => _editTracks;
      set
      {
        if (_editTracks)
        {
          _parent.LayoutController.Items.Remove(_previewTrack);
        }
        _editTracks = value;
        if (_editTracks)
        {
          _parent.LayoutController.InsertItemTypeSorted(_previewTrack);
        }
        NotifyPropertyChanged(nameof(EditTracks));
      }
    }

    public bool Horizontal { get; set; } = true;

    public bool Vertical { get; set; }

    public bool Diagonal { get; set; }

    public bool AntiDiagonal { get; set; }

    #endregion

    #region Placement Trains

    public bool PlaceTrain
    {
      get => _placeTrain;
      set
      {
        _placeTrain = value;
        NotifyPropertyChanged(nameof(PlaceTrain));
      }
    }

    #endregion

    #region Placement Signals

    public bool EditSignals
    {
      get => _editSignals;
      set
      {
        _editSignals = value;
        NotifyPropertyChanged(nameof(EditSignals));
      }
    }

    #endregion

    #region Properties

    public bool ValidPosition
    {
      get => _validPosition;
      set
      { }
    }

    public Brush CurserColor
    {
      get => _curserColor;
      private set
      {
        _curserColor = value;
        NotifyPropertyChanged(nameof(CurserColor));
      }
    }

    public ObservableCollection<ItemViewModel> SelectedItems
    {
      get => _selectedItems;
      private set
      {
        _selectedItems = value;
      }
    }

    #endregion

  }
}
