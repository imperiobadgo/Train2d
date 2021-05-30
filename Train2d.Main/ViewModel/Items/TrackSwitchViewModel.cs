using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Train2d.Main.Commands;
using Train2d.Main.Controls;
using Train2d.Model;
using Train2d.Model.Items;

namespace Train2d.Main.ViewModel.Items
{
  public class TrackSwitchViewModel : ItemGenericViewModel<TrackSwitch>
  {

    #region Construct

    public TrackSwitchViewModel() : this(new TrackSwitch())
    { }

    public TrackSwitchViewModel(TrackSwitch newSwitch)
    {
      ChangeSwitchCommand = new DelegateCommand(ChangeSwitchCommandExecute);
      SetItem(newSwitch);
      DisplayOrder = 2;
      MainColor = Brushes.Pink;
    }

    protected override void OnItemSet(Item item)
    {

    }

    #endregion

    #region Methods

    public void Configure(Guid? trackId, List<Guid> adjacentTracks)
    {
      TrackId = trackId;
      AdjacentTrackIds = adjacentTracks;
      NotifyPropertyChanged(nameof(AdjacentTrackIds));
      NotifyPropertyChanged(nameof(SelectedAdjacentTrackId));
      NotifyPropertyChanged(nameof(SelectedPosition));
      NotifyPropertyChanged(nameof(SelectedCoordinate));
      NotifyPropertyChanged(nameof(SelectedAngle));
      NotifyPropertyChanged(nameof(SelectedXScale));
    }

    public override void OnSelectMain(LayoutController controller, LayoutViewModel layout)
    {
      ChangeSwitch();
    }

    public List<TrackViewModel> GetTracks(LayoutController controller)
    {
      List<TrackViewModel> tracks = new List<TrackViewModel>();
      if (TrackId.HasValue)
      {
        ItemViewModel mainTrackItem = controller.GetLayoutItemFromId(TrackId.Value);
        if (mainTrackItem is TrackViewModel mainTrack)
        {
          tracks.Add(mainTrack);
        }

      }
      if (AdjacentTrackIds.Count > 0)
      {
        ItemViewModel adjacentTrackItem = controller.GetLayoutItemFromId(AdjacentTrackIds.First());
        if (adjacentTrackItem is TrackViewModel adjacentTrack)
        {
          tracks.Add(adjacentTrack);
        }
      }

      return tracks;
    }

    public bool ChangeSwitch()
    {
      if (_layout == null)
      {
        return false;
      }
      List<Guid> adjacentTracks = Item().AdjacentTrackIds.ToList();
      Guid firstTrackId = adjacentTracks.FirstOrDefault();
      if (firstTrackId == null)
      {
        return false;
      }
      //rotate through all possible tracks
      adjacentTracks.Remove(firstTrackId);
      adjacentTracks.Add(firstTrackId);
      _layout.GetCommandController().AddCommand(new ConfigureTrackSwitchCommand(_layout, this, TrackId, adjacentTracks));
      return true;
    }

    #endregion

    #region Commands

    public DelegateCommand ChangeSwitchCommand { get; private set; }

    private void ChangeSwitchCommandExecute(object o)
    {
      if (ChangeSwitch())
      {
        _layout.GetCommandController().ExecuteNewCommands();
      }
    }

    #endregion

    #region Properties

    public Guid? TrackId
    {
      get => Item().TrackId;
      private set
      {
        Item().TrackId = value;
        NotifyPropertyChanged(nameof(TrackId));
      }
    }


    public List<Guid> AdjacentTrackIds
    {
      get => Item().AdjacentTrackIds;
      private set
      {
        Item().AdjacentTrackIds = value;
        NotifyPropertyChanged(nameof(AdjacentTrackIds));
      }
    }

    public Guid? SelectedAdjacentTrackId => Item().AdjacentTrackIds.FirstOrDefault();

    public Coordinate? SelectedCoordinate
    {
      get
      {
        if (_layout is null)
        {
          return null;
        }
        TrackViewModel track = (TrackViewModel)_layout.LayoutController.GetLayoutItemFromId(SelectedAdjacentTrackId);
        return track == null ? null : track.Coordinate;
      }
    }

    public Vector SelectedPosition
    {
      get
      {
        if (_layout is null)
        {
          return new Vector(0, 0);
        }
        TrackViewModel track = (TrackViewModel)_layout.LayoutController.GetLayoutItemFromId(SelectedAdjacentTrackId);
        return track == null ? new Vector(0, 0) : track.Position;
      }
    }

    public double SelectedAngle
    {
      get
      {
        if (_layout is null)
        {
          return 0;
        }
        TrackViewModel track = (TrackViewModel)_layout.LayoutController.GetLayoutItemFromId(SelectedAdjacentTrackId);
        return track == null ? 0 : track.Angle;
      }
    }

    public double SelectedXScale
    {
      get
      {
        if (_layout is null)
        {
          return 0;
        }
        TrackViewModel track = (TrackViewModel)_layout.LayoutController.GetLayoutItemFromId(SelectedAdjacentTrackId);
        return track == null ? 0 : track.XScale;
      }
    }

    #endregion


  }
}
