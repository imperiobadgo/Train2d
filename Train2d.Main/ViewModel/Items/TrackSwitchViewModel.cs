using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Train2d.Model.Items;

namespace Train2d.Main.ViewModel.Items
{
  public class TrackSwitchViewModel : ItemGenericViewModel<TrackSwitch>
  {
    #region Construct

    public TrackSwitchViewModel() : this(new TrackSwitch())
    {}

    public TrackSwitchViewModel(TrackSwitch newSwitch)
    {
      SetItem(newSwitch);
      DisplayOrder = 2;
      MainColor = Brushes.Pink;
    }

    #endregion

    #region Methods

    public void Configure(TrackViewModel track, List<Guid> adjacentTracks)
    {
      Track = track?.Id;
      AdjacentTracks = adjacentTracks;
    }

    #endregion

    #region Properties

    public Guid? Track
    {
      get => Item().Track;
      private set
      {
        Item().Track = value;
        NotifyPropertyChanged(nameof(Track));
      }
    }


    public List<Guid> AdjacentTracks
    {
      get => Item().AdjacentTracks;
      private set
      {
        Item().AdjacentTracks = value;
        NotifyPropertyChanged(nameof(AdjacentTracks));
      }
    }

    #endregion


  }
}
