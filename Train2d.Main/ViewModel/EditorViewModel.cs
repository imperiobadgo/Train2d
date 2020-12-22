using Train2d.Main.Commands;
using Train2d.Main.ViewModel.Items;
using Train2d.Model.Converter;

namespace Train2d.Main.ViewModel
{
  public class EditorViewModel : ViewModelBase
  {
    #region Attributes

    private readonly LayoutViewModel _parent;
    private bool _placeTracks;

    #endregion

    #region Construct

    public EditorViewModel(LayoutViewModel parent)
    {
      _parent = parent;
      _parent.Settings.SetOnMouseLeftButtonDownAction(OnMouseLeftButtonUp);
    }

    #endregion

    #region Methods

    private void OnMouseLeftButtonUp()
    {
      if (PlaceTracks)
      {
        TrackViewModel newTrack = new TrackViewModel();
        CreateItemCommand createCommand = new CreateItemCommand(_parent, newTrack);
        _parent.GetCommandController().AddCommandAndExecute(createCommand);
        PositionItemCommand positionItem = new PositionItemCommand(_parent, newTrack, _parent.Settings.MousePosition.ToCoordinate());
        _parent.GetCommandController().AddCommandAndExecute(positionItem);
        OrientateTrackCommand orientateCommand;
        if (Vertical)
        {
          orientateCommand = new OrientateTrackCommand(_parent, newTrack, Model.TrackOrientation.Vertical);
        }
        else if (Diagonal)
        {
          orientateCommand = new OrientateTrackCommand(_parent, newTrack, Model.TrackOrientation.Diagonal);
        }
        else if (AntiDiagonal)
        {
          orientateCommand = new OrientateTrackCommand(_parent, newTrack, Model.TrackOrientation.AntiDiagonal);
        }
        else
        {
          //Backup case
          orientateCommand = new OrientateTrackCommand(_parent, newTrack, Model.TrackOrientation.Horizontal);
        }
        _parent.GetCommandController().AddCommandAndExecute(orientateCommand);
      }
    }

    #endregion

    #region Properties

    public bool PlaceTracks
    {
      get => _placeTracks; set
      {
        _placeTracks = value;
        NotifyPropertyChanged(nameof(PlaceTracks));
      }
    }

    public bool Horizontal { get; set; }

    public bool Vertical { get; set; }

    public bool Diagonal { get; set; }

    public bool AntiDiagonal { get; set; }

    #endregion

  }
}
