using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Train2d.Model;
using Train2d.Model.Items;

namespace Train2d.Main.ViewModel.Items
{
  public class TrackSwitchViewModel : ItemGenericViewModel<TrackSwitch>
  {

    #region Attributes

    private Coordinate? _endCoordinate;
    private LayoutController _controller;
    #endregion

    #region Construct

    public TrackSwitchViewModel() : this(new TrackSwitch())
    { }

    public TrackSwitchViewModel(TrackSwitch newSwitch)
    {
      SetItem(newSwitch);
      DisplayOrder = 2;
      MainColor = Brushes.Pink;
    }

    protected override void OnItemSet(Item item)
    {

    }

    #endregion

    #region Methods

    public void SetController(LayoutController controller)
    {
      _controller = controller;
    }

    public void Configure(TrackViewModel track, List<Guid> adjacentTracks)
    {
      TrackId = track?.Id;
      AdjacentTrackIds = adjacentTracks;
    }

    public override void OnSelectMain(LayoutController controller)
    {
      List<Guid> adjacentTracks = Item().AdjacentTrackIds;
      var firstTrackId = adjacentTracks.FirstOrDefault();
      if (firstTrackId == null)
      {
        return;
      }
      //rotate through all possible tracks
      adjacentTracks.Remove(firstTrackId);
      adjacentTracks.Add(firstTrackId);
      Console.WriteLine($"Switched {DateTime.UtcNow} from {firstTrackId} to {adjacentTracks.FirstOrDefault()}");
      NotifyPropertyChanged(nameof(AdjacentTrackIds));
      NotifyPropertyChanged(nameof(SelectedAdjacentTrackId));
      NotifyPropertyChanged(nameof(SelectedAngle));
      NotifyPropertyChanged(nameof(SelectedXScale));
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

    public Guid? SelectedAdjacentTrackId
    {
      get => Item().AdjacentTrackIds.FirstOrDefault();
    }

    public Coordinate? SelectedCoordinate
    {
      get
      {
        if (_controller is null)
        {
          return null;
        }
        TrackViewModel track = (TrackViewModel)_controller.GetLayoutItemFromId(SelectedAdjacentTrackId);
        if (track == null)
        {
          return null;
        }
        return track.Coordinate;
      }
    }
    public double SelectedAngle
    {
      get
      {
        if (_controller is null)
        {
          return 0;
        }
        TrackViewModel track = (TrackViewModel)_controller.GetLayoutItemFromId(SelectedAdjacentTrackId);
        if (track == null)
        {
          return 0;
        }
        return track.Angle;
      }
    }

    public double SelectedXScale
    {
      get
      {
        if (_controller is null)
        {
          return 0;
        }
        TrackViewModel track = (TrackViewModel)_controller.GetLayoutItemFromId(SelectedAdjacentTrackId);
        if (track == null)
        {
          return 0;
        }
        return track.XScale;
      }
    }

    #endregion


  }
}
