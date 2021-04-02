using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Train2d.Main.Commands;
using Train2d.Main.ViewModel.Items;
using Train2d.Model;
using Train2d.Model.Converter;

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

    #endregion

    #region Construct

    public EditorViewModel(LayoutViewModel parent)
    {
      _parent = parent;
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
        //add previewTrack to the layout for switch updating, so the previewtrack is used for checking, whether a switch should be added
        //The advantage is that in this way the commands of creating the new track and changing all switches can be executed together.
        _parent.LayoutController.AddLayoutItem(_previewTrack);
        _parent.LayoutController.AddItemToLayout(_mouseCoordinate, _previewTrack);

        addedTrack = new TrackViewModel();
        TrackOrientation orientation = GetSelectedTrackOrientation();
        commands.Add(new CreateItemCommand(_parent, addedTrack));
        commands.Add(new PositionItemOnLayoutCommand(_parent, addedTrack, _mouseCoordinate));
        commands.Add(new OrientateTrackCommand(_parent, addedTrack, orientation));
      }

      if (PlaceTrain)
      {
        var tracksAtCoordinate = _parent.LayoutController.GetLayoutItems(_mouseCoordinate).OfType<TrackViewModel>().FirstOrDefault();

        TrainViewModel newTrain = new TrainViewModel();
        commands.Add(new CreateItemCommand(_parent, newTrain));
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

      if (EditSignals)
      {
        SignalViewModel newSignal = new SignalViewModel();
        commands.Add(new CreateItemCommand(_parent, newSignal));
        commands.Add(new PositionItemOnLayoutCommand(_parent, newSignal, _mouseCoordinate));
      }

      List<CommandBase> switchCommands = new List<CommandBase>();
      UpdateSwitchesOnsurroundingTracks(_mouseCoordinate, switchCommands);

      if (EditTracks)
      {
        //dont forget to remove the previewtrack after switch updating
        _parent.LayoutController.RemoveItemFromLayout(_mouseCoordinate, _previewTrack);
        _parent.LayoutController.RemoveLayoutItem(_previewTrack);
      }

      commands.AddRange(switchCommands);
      if (commands.Count > 0)
      {
        CommandChain commandChain = new CommandChain(commands);
        _parent.GetCommandController().AddCommandAndExecute(commandChain);
      }

      UpdateValidPosition();
    }

    /// <summary>
    /// Check at all possible tracks around the changed position, whether switches need to be updated.
    /// </summary>
    /// <param name="switchCommands">All commands for creating, updating and deleting switches.</param>
    private void UpdateSwitchesOnsurroundingTracks(Coordinate centerCoordinate, List<CommandBase> switchCommands)
    {
      List<TrackViewModel> checkTracks = new List<TrackViewModel>();
      for (int x = centerCoordinate.X - 1; x <= centerCoordinate.X + 1; x++)
      {
        for (int y = centerCoordinate.Y - 1; y <= centerCoordinate.Y + 1; y++)
        {
          checkTracks.AddRange(_parent.LayoutController.GetLayoutItems(new Coordinate(x, y)).OfType<TrackViewModel>());
        }
      }
      foreach (TrackViewModel checkTrack in checkTracks)
      {
        AddSwitchIfNecessary(checkTrack, switchCommands);
      }
    }

    /// <summary>
    /// Check around the track, whether a switch should be added, updated or removed.
    /// </summary>
    /// <param name="switchCommands">All commands for creating, updating and deleting switches.</param>
    private void AddSwitchIfNecessary(TrackViewModel checkTrack, List<CommandBase> switchCommands)
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
      for (int x = addedX - 1; x <= addedX + 1; x++)
      {
        for (int y = addedY - 1; y <= addedY + 1; y++)
        {
          if (x == addedX && y == addedY)
          {
            continue;
          }
          IEnumerable<TrackViewModel> tracks = _parent.LayoutController.GetLayoutItems(new Coordinate(x, y)).OfType<TrackViewModel>();
          foreach (TrackViewModel track in tracks)
          {
            if (track.ContainsCoordinate(checkTrack.Coordinate.Value))
            {
              adjacentTracksA.Add(track);
            }
            if (track.ContainsCoordinate(checkTrack.EndCoordinate.Value))
            {
              adjacentTracksB.Add(track);
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
      if (adjacentTracks.Count < 2)
      {
        return;
      }
      List<Guid> trackGuids = adjacentTracks.Select(x => x.Id.Value).ToList();
      List<CommandBase> commands = new List<CommandBase>();
      TrackSwitchViewModel trackSwitch = new TrackSwitchViewModel();
      commands.Add(new CreateItemCommand(_parent, trackSwitch));
      commands.Add(new PositionItemOnLayoutCommand(_parent, trackSwitch, checkCoordinate));
      commands.Add(new ConfigureTrackSwitchCommand(_parent, trackSwitch, checkTrack, trackGuids));
      switchCommands.Add(new CommandChain(commands));
    }

    private void OnSelectSub()
    {
      if (EditTracks)
      {
        List<ItemViewModel> items = _parent.LayoutController.GetLayoutItems(_mouseCoordinate);
        foreach (var item in items)
        {
          DeleteItemCommand deleteCommand = new DeleteItemCommand(_parent, item);
          _parent.GetCommandController().AddCommandAndExecute(deleteCommand);
        }
      }

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
      var itemAtCoordinate = _parent.LayoutController.GetLayoutItems(_mouseCoordinate);
      if (EditTracks)
      {
        _validPosition = !itemAtCoordinate.OfType<TrackViewModel>().Any(x => Equals(x.Orientation, GetSelectedTrackOrientation()));
        UpdatePreviewTrack(_validPosition);
      }

      if (PlaceTrain || EditSignals)
      {
        _validPosition = itemAtCoordinate.OfType<TrackViewModel>().Any();
        UpdateCurser(_validPosition);
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

    #region Signals

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

  }
}
