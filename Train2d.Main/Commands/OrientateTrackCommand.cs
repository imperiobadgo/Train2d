using Train2d.Main.ViewModel;
using Train2d.Main.ViewModel.Items;
using Train2d.Model;

namespace Train2d.Main.Commands
{
  class OrientateTrackCommand : CommandItemBase<TrackViewModel>
  {
    private TrackOrientation _newOrientation;
    private TrackOrientation _oldOrientation;

    public OrientateTrackCommand(LayoutViewModel viewModel, TrackViewModel track, TrackOrientation orientation) : base(viewModel, track)
    {
      _newOrientation = orientation;
      _oldOrientation = track.Orientation;
    }

    protected override bool Execute()
    {
      _item.SetOrientation(_newOrientation);
      return true;
    }

    protected override void Undo()
    {
      _item.SetOrientation(_oldOrientation);
    }

    public override string ToString()
    {
      return $"Oriented item on {_item.Coordinate} to {_newOrientation}. Type: {_item}";
    }
  }
}
