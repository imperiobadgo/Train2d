using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;
using Train2d.Model;
using Train2d.Model.Items;

namespace Train2d.Main.ViewModel.Items
{
  [DebuggerDisplay("Track {Orientation} x={Coordinate.Value.X} y={Coordinate.Value.Y} ")]
  public class TrackViewModel : ItemGenericViewModel<Track>
  {
    #region Attributes

    private Coordinate? _endCoordinate;

    #endregion

    #region Construct

    public TrackViewModel() : this(new Track())
    {}

    public TrackViewModel(Track newTrack)
    {
      SetItem(newTrack);
      DisplayOrder = 1;
      MainColor = Brushes.Aqua;
    }

    protected override void OnItemSet(Item item)
    {
      UpdateEndCoordinate();
    }
    #endregion

    #region Methods

    public void SetOrientation(TrackOrientation newOrientation)
    {
      Orientation = newOrientation;
      NotifyPropertyChanged(nameof(Angle));
      NotifyPropertyChanged(nameof(XScale));
    }

    public bool ContainsCoordinate(Coordinate testCoordinate)
    {
      if (!Coordinate.HasValue || !EndCoordinate.HasValue)
      {
        return false;
      }
      return Coordinate.Value.Equals(testCoordinate) || EndCoordinate.Value.Equals(testCoordinate);
    }

    public Coordinate? GetOtherCoordinate(Coordinate testCoordinate)
    {
      if (!Coordinate.HasValue || !EndCoordinate.HasValue)
      {
        return null;
      }
      
      if (Coordinate.Value.Equals(testCoordinate))
      {
        return EndCoordinate.Value;
      }
      else if(EndCoordinate.Value.Equals(testCoordinate))
      {
        return Coordinate.Value;
      }
      return null;
    }

    private void UpdateEndCoordinate()
    {
      if (!Coordinate.HasValue)
      {
        _endCoordinate = null;
        return;
      }
      int offsetX = 0, offsetY = 0;
      switch (Orientation)
      {
        case TrackOrientation.Horizontal:
          offsetX = 1;
          break;
        case TrackOrientation.Vertical:
          offsetY = 1;
          break;
        case TrackOrientation.Diagonal:
          offsetX = 1;
          offsetY = 1;
          break;
        case TrackOrientation.AntiDiagonal:
          offsetX = -1;
          offsetY = 1;
          break;
        default:
          break;
      }
      _endCoordinate = new Coordinate(Coordinate.Value.X + offsetX, Coordinate.Value.Y + offsetY);
      NotifyPropertyChanged(nameof(EndCoordinate));
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

    /// <summary>
    /// 7  0  1
    /// 6     2
    /// 5  4  3
    /// </summary>
    public int GetDirectionInA()
    {
      int directionA = 0;
      switch (Orientation)
      {
        case TrackOrientation.Horizontal:
          directionA = 6;
          break;
        case TrackOrientation.Vertical:
          directionA = 0;
          break;
        case TrackOrientation.Diagonal:
          directionA = 5;
          break;
        case TrackOrientation.AntiDiagonal:
          directionA = 7;
          break;
        default:
          break;
      }
      return directionA;
    }

    /// <summary>
    /// 7  0  1
    /// 6     2
    /// 5  4  3
    /// </summary>
    public int GetDirectionInB()
    {
      int directionB = 0;
      switch (Orientation)
      {
        case TrackOrientation.Horizontal:
          directionB = 2;
          break;
        case TrackOrientation.Vertical:
          directionB = 4;
          break;
        case TrackOrientation.Diagonal:
          directionB = 1;
          break;
        case TrackOrientation.AntiDiagonal:
          directionB = 3;
          break;
        default:
          break;
      }
      return directionB;
    }

    #endregion

    #region Properties

    public Coordinate? EndCoordinate
    {
      get => _endCoordinate;
      private set { }
    }

    public TrackOrientation Orientation
    {
      get => Item().Orientation;
      private set
      {
        Item().Orientation = value;
        UpdateEndCoordinate();
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
            return 45;
          case TrackOrientation.AntiDiagonal:
            return 135;
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
