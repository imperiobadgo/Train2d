using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Train2d.Main.Commands;
using Train2d.Model;
using Train2d.Model.Items;

namespace Train2d.Main.ViewModel.Items
{
  public class TrainViewModel : ItemGenericViewModel<Train>, IUpdateableItem
  {
    #region Attributes

    private int _coolDown;
    private int _resetCoolDown = 20;

    #endregion

    #region Construct

    public TrainViewModel() : this(new Train())
    { }

    public TrainViewModel(Train newTrain)
    {
      SetItem(newTrain);
      DisplayOrder = 5;
      MainColor = Brushes.DarkBlue;
      _coolDown = _resetCoolDown;
    }

    #endregion

    #region Methods

    public void Update(LayoutViewModel layout, float deltaTime)
    {
      if (!Coordinate.HasValue)
      {
        return;
      }
      if (_coolDown > 0)
      {
        _coolDown -= 1;
        return;
      }
      _coolDown = _resetCoolDown;

      List<TrackViewModel> possibleTracks = layout.LayoutController.GetAdjacentTracksOnPosition(Coordinate.Value);
      if (possibleTracks.Count == 0)
      {
        return;
      }
      List<Tuple<TrackViewModel, int, Coordinate>> resultTracks = new List<Tuple<TrackViewModel, int, Coordinate>>();
      List<Tuple<int, Coordinate>> coordsInDirection = GetCoordinatesInDirection();
      foreach (TrackViewModel track in possibleTracks)
      {
        foreach (Tuple<int, Coordinate> coordinate in coordsInDirection)
        {
          if (track.ContainsCoordinate(coordinate.Item2))
          {
            if (!resultTracks.Any(x => Equals(x.Item1, track)))
            {
              resultTracks.Add(Tuple.Create(track, coordinate.Item1, coordinate.Item2));
            }
          }
        }
      }

      if (resultTracks.Count == 1)
      {
        List<CommandBase> commands = new List<CommandBase>();
        commands.Add(new RemoveItemFromLayoutCommand(layout, this));
        commands.Add(new PositionItemOnLayoutCommand(layout, this, resultTracks[0].Item3));
        commands.Add(new SetTrainDirectionCommand(layout, this, resultTracks[0].Item2));
        CommandChain chain = new CommandChain(commands);

        layout.GetCommandController().AddCommand(chain);
      }
    }


    public List<Tuple<int, Coordinate>> GetCoordinatesInDirection()
    {
      List<Tuple<int, Coordinate>> coordinates = new List<Tuple<int, Coordinate>>();
      List<int> possibleDirections = new List<int>();
      for (int i = Direction - 1; i < Direction + 2; i++)
      {
        if (i < 0)
        {
          possibleDirections.Add(i + Train.DirectionRange);
        }
        else if (i > Train.DirectionRange - 1)
        {
          possibleDirections.Add(i - Train.DirectionRange);
        }
        else
        {
          possibleDirections.Add(i);
        }
      }
      foreach (int dir in possibleDirections)
      {
        Coordinate? possibleCoord = Item().GetCoordinateInDirection(dir);
        if (possibleCoord.HasValue)
        {
          coordinates.Add(Tuple.Create(dir, possibleCoord.Value));
        }
      }

      return coordinates;
    }

    #endregion

    #region Properties

    public int Direction
    {
      get => Item().Direction;
      set
      {
        Item().Direction = value;
        NotifyPropertyChanged(nameof(Direction));
      }
    }

    #endregion

  }
}
