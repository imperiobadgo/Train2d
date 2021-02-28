using System.Collections.Generic;
using System.Linq;
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
      List<CommandBase> commands = new List<CommandBase>();
      if (EditTracks)
      {
        TrackViewModel newTrack = new TrackViewModel();
        TrackOrientation orientation = GetSelectedTrackOrientation();
        commands.Add(new CreateItemCommand(_parent, newTrack));
        commands.Add(new PositionItemCommand(_parent, newTrack, _mouseCoordinate));
        commands.Add(new OrientateTrackCommand(_parent, newTrack, orientation));
      }

      if (PlaceTrain)
      {
        TrainViewModel newTrain = new TrainViewModel();
        commands.Add(new CreateItemCommand(_parent, newTrain));
        commands.Add(new PositionItemCommand(_parent, newTrain, _mouseCoordinate));
      }

      if (EditSignals)
      {
        SignalViewModel newSignal = new SignalViewModel();
        commands.Add(new CreateItemCommand(_parent, newSignal));
        commands.Add(new PositionItemCommand(_parent, newSignal, _mouseCoordinate));
      }

      if (commands.Count > 0)
      {
        CommandChain commandChain = new CommandChain(commands);
        _parent.GetCommandController().AddCommandAndExecute(commandChain);
      }

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
      var itemAtCoordinate = _parent.LayoutController.GetLayoutItems(_mouseCoordinate);
      if (EditTracks)
      {
        _validPosition = !itemAtCoordinate.OfType<TrackViewModel>().Any();
        UpdatePreviewTrack();
      }

      if (PlaceTrain)
      {
        _validPosition = itemAtCoordinate.OfType<TrackViewModel>().Any();
      }
      NotifyPropertyChanged(nameof(ValidPosition));
    }


    #endregion

    public bool ValidPosition
    {
      get => _validPosition;
      set
      { }
    }

    #region Placement Tracks

    private void UpdatePreviewTrack()
    {
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
          _parent.LayoutController.Items.Add(_previewTrack);
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
