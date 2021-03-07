using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Train2d.Main.ViewModel;
using Train2d.Main.ViewModel.Items;

namespace Train2d.Main.Commands
{
  public class ConfigureTrackSwitchCommand : CommandItemBase<TrackSwitchViewModel>
  {
    private TrackViewModel _oldTrack;
    private TrackViewModel _newTrack;
    private List<Guid> _oldAdjacentTracks;
    private List<Guid> _newAdjacentTracks;

    public ConfigureTrackSwitchCommand(LayoutViewModel viewModel, TrackSwitchViewModel trackSwitch, TrackViewModel track, List<Guid> adjacentTracks) : base(viewModel, trackSwitch)
    {
      _oldTrack = viewModel.LayoutController.GetLayoutItemFromId(trackSwitch.Track) as TrackViewModel; // as equals TryCast in vb
      _newTrack = track;
      _oldAdjacentTracks = trackSwitch.AdjacentTracks;
      _newAdjacentTracks = adjacentTracks;
    }

    protected override bool Execute()
    {
      _item.Configure(_newTrack, _newAdjacentTracks);
      return true;
    }

    protected override void Undo()
    {
      _item.Configure(_oldTrack, _oldAdjacentTracks);
    }

  }
}
