﻿using System.Collections.Generic;
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
    private bool _placeTracks;
    private bool _placeTrain;
    private Coordinate _mouseCoordinate;
    private bool _validPosition;

    #endregion

    #region Construct

    public EditorViewModel(LayoutViewModel parent)
    {
      _parent = parent;
      _parent.Settings.SetOnMouseLeftButtonDownAction(OnMouseLeftButtonUp);
      _parent.Settings.SetOnMouseMoveAction(OnMouseMove);
    }



    #endregion

    #region Methods

    private void OnMouseLeftButtonUp()
    {
      if (PlaceTracks)
      {
        TrackViewModel newTrack = new TrackViewModel();
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
        List<CommandBase> commands = new List<CommandBase>();
        commands.Add(new CreateItemCommand(_parent, newTrack));
        commands.Add(new PositionItemCommand(_parent, newTrack, _mouseCoordinate));
        commands.Add(new OrientateTrackCommand(_parent, newTrack, orientation));
        CommandChain commandChain = new CommandChain(commands);
        _parent.GetCommandController().AddCommandAndExecute(commandChain);
      }
      if (PlaceTrain)
      {

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
      if (PlaceTracks)
      {
        _validPosition = !itemAtCoordinate.OfType<TrackViewModel>().Any();
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

    public bool PlaceTracks
    {
      get => _placeTracks;
      set
      {
        _placeTracks = value;
        NotifyPropertyChanged(nameof(PlaceTracks));
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

  }
}
