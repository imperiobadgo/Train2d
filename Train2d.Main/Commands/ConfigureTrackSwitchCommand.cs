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
    private Guid? _oldTrack;
    private Guid? _newTrack;
    private List<Guid> _oldAdjacentTracks;
    private List<Guid> _newAdjacentTracks;

    public ConfigureTrackSwitchCommand(LayoutViewModel viewModel, TrackSwitchViewModel trackSwitch, Guid? trackId, List<Guid> adjacentTracks) : base(viewModel, trackSwitch)
    {
      _oldTrack = trackSwitch.TrackId;
      _newTrack = trackId;
      _oldAdjacentTracks = trackSwitch.AdjacentTrackIds;
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

    public override string ToString()
    {
      return $"Configured from {_oldAdjacentTracks.Count} to {_newAdjacentTracks.Count}. Type: {_item}";
    }

  }
}
