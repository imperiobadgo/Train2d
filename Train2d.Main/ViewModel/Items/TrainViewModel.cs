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
    private int _resetCoolDown = 10;

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

    protected override void OnItemSet(Item item)
    {

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
      //track, track direction, targetposition
      List<Tuple<TrackViewModel, int, Coordinate>> resultTracks = new List<Tuple<TrackViewModel, int, Coordinate>>();
      //direction, position in direction
      List<Tuple<int, Coordinate>> coordsInDirection = GetCoordinatesInDirection(Coordinate, Direction);
      foreach (TrackViewModel track in possibleTracks)
      {
        foreach (Tuple<int, Coordinate> coordinate in coordsInDirection)
        {
          if (track.ContainsCoordinate(Coordinate.Value) && track.ContainsCoordinate(coordinate.Item2))
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
        Coordinate targetCoordinate = resultTracks[0].Item3;
        int targetDirection = resultTracks[0].Item2;
        List<ItemViewModel> itemsOnTargetPositions = layout.LayoutController.GetLayoutItems(targetCoordinate);
        bool mustStop = itemsOnTargetPositions.OfType<SignalViewModel>().Any(signal => signal.Direction == targetDirection && signal.State == Signal.STATE_HOlD);
        if (mustStop)
        {
          return;
        }
        List<CommandBase> commands = new List<CommandBase>();
        commands.Add(new RemoveItemFromLayoutCommand(layout, this));
        commands.Add(new PositionItemOnLayoutCommand(layout, this, targetCoordinate));
        commands.Add(new SetTrainDirectionCommand(layout, this, targetDirection));
        CommandChain chain = new CommandChain(commands);

        layout.GetCommandController().AddCommand(chain);
      }
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
