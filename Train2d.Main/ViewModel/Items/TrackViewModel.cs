using Train2d.Model;
using Train2d.Model.Items;

namespace Train2d.Main.ViewModel.Items
{
  public class TrackViewModel : ItemGenericViewModel<Track>
  {
    #region Construct

    public TrackViewModel()
    {
      SetItem(new Track());
    }

    #endregion

    #region Methods

    public void SetOrientation(TrackOrientation newOrientation)
    {
      Orientation = newOrientation;
      NotifyPropertyChanged(nameof(Angle));
    }

    #endregion

    #region Properties

    public TrackOrientation Orientation
    {
      get => Item().Orientation;
      private set
      {
        Item().Orientation = value;
        NotifyPropertyChanged(nameof(Orientation));
      }
    }

    public double Angle
    {
      get
      {
        switch (Orientation)
        {
          case TrackOrientation.Horizontal:
            return 0;
          case TrackOrientation.Vertical:
            return 90;
          case TrackOrientation.Diagonal:
            return 135;
          case TrackOrientation.AntiDiagonal:
            return 45;
          default:
            return 0;
        }
      }
      private set { }
    }

    #endregion
  }
}
