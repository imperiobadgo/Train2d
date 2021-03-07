using System.Collections.Generic;
using System.Windows.Media;
using Train2d.Model;
using Train2d.Model.Items;

namespace Train2d.Main.ViewModel.Items
{
  public class TrackViewModel : ItemGenericViewModel<Track>
  {
    #region Construct

    public TrackViewModel() : this(new Track())
    {}

    public TrackViewModel(Track newTrack)
    {
      SetItem(newTrack);
      DisplayOrder = 1;
      MainColor = Brushes.Aqua;
    }

    #endregion

    #region Methods

    public void SetOrientation(TrackOrientation newOrientation)
    {
      Orientation = newOrientation;
      NotifyPropertyChanged(nameof(Angle));
      NotifyPropertyChanged(nameof(XScale));
    }

    public List<TrackOrientation> GetAdjacentTrackOrientations()
    {
      List<TrackOrientation> orientations = new List<TrackOrientation>();
      switch (Orientation)
      {
        case TrackOrientation.Horizontal:
          orientations.Add(TrackOrientation.Horizontal);
          orientations.Add(TrackOrientation.Diagonal);
          orientations.Add(TrackOrientation.AntiDiagonal);
          break;
        case TrackOrientation.Vertical:
          orientations.Add(TrackOrientation.Vertical);
          orientations.Add(TrackOrientation.Diagonal);
          orientations.Add(TrackOrientation.AntiDiagonal);
          break;
        case TrackOrientation.Diagonal:
          orientations.Add(TrackOrientation.Horizontal);
          orientations.Add(TrackOrientation.Vertical);
          orientations.Add(TrackOrientation.Diagonal);          
          break;
        case TrackOrientation.AntiDiagonal:
          orientations.Add(TrackOrientation.Horizontal);
          orientations.Add(TrackOrientation.Vertical);
          orientations.Add(TrackOrientation.AntiDiagonal);
          break;
        default:
          break;
      }
      return orientations;
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

    public double XScale
    {
      get
      {
        switch (Orientation)
        {
          case TrackOrientation.Horizontal:
            return 1;
          case TrackOrientation.Vertical:
            return 1;
          case TrackOrientation.Diagonal:
            return 1.414;
          case TrackOrientation.AntiDiagonal:
            return 1.414;
          default:
            return 1;
        }
      }
      private set { }
    }

    #endregion
  }
}
