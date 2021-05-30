using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Train2d.Main.Commands;
using Train2d.Main.Controls;
using Train2d.Model;
using Train2d.Model.Items;

namespace Train2d.Main.ViewModel.Items
{
  public class TrainViewModel : ItemGenericViewModel<Train>, IUpdateableItem
  {
    #region Attributes

    private float _currentPositionInCell;

    #endregion

    #region Construct

    public TrainViewModel() : this(new Train())
    { }

    public TrainViewModel(Train newTrain)
    {
      InvertDirectionCommand = new DelegateCommand(InvertDirectionCommandExecute);
      SetItem(newTrain);
      DisplayOrder = 5;
      MainColor = Brushes.DarkBlue;
    }

    protected override void OnItemSet(Item item)
    {

    }

    #endregion

    #region Methods

    public void Update(float deltaTime)
    {
      if (!Coordinate.HasValue || _layout == null)
      {
        return;
      }
      if (ForceStop)
      {
        return;
      }
      if (_currentPositionInCell < Model.Coordinate.CELLSIZE)
      {
        _currentPositionInCell += deltaTime * Item().Speed;
        return;
      }
      _currentPositionInCell = 0;

      List<TrackViewModel> possibleTracks = _layout.LayoutController.GetAdjacentTracksOnPosition(Coordinate.Value);
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
        List<ItemViewModel> itemsOnTargetPosition = _layout.LayoutController.GetLayoutItems(targetCoordinate);
        bool mustStop = itemsOnTargetPosition.OfType<SignalViewModel>().Any(signal => signal.Direction == targetDirection && signal.State == Signal.STATE_HOlD);
        if (mustStop)
        {
          return;
        }
        List<ItemViewModel> itemsOnCurrentPosition = _layout.LayoutController.GetLayoutItems(Coordinate.Value);
        List<SignalViewModel> signalsOnCurrentPosition = itemsOnCurrentPosition.OfType<SignalViewModel>().Where(signal => signal.Direction == targetDirection && signal.State == Signal.STATE_GO).ToList();
        List<CommandBase> commands = new List<CommandBase>();        
        commands.Add(new RemoveItemFromLayoutCommand(_layout, this));
        commands.Add(new PositionItemOnLayoutCommand(_layout, this, targetCoordinate));
        commands.Add(new SetTrainDirectionCommand(_layout, this, targetDirection));

        foreach (var currentSignal in signalsOnCurrentPosition)
        {
          commands.Add(new SetSignalStateCommand(_layout, currentSignal, Signal.STATE_HOlD));
        }

        CommandChain chain = new CommandChain(commands);

        _layout.GetCommandController().AddCommand(chain);
      }
    }

    #endregion

    #region Userproperties

    public bool ForceStopChanger
    {
      get => Item().ForceStop;
      set
      {
        if (_layout == null)
        {
          return;
        }
        _layout.GetCommandController().AddCommandAndExecute(new SetTrainForceStopStateCommand(_layout, this, !Item().ForceStop));
      }
    }

    #endregion

    #region Commands

    public DelegateCommand InvertDirectionCommand { get; private set; }

    private void InvertDirectionCommandExecute(object o)
    {
      int invertedDirection = Model.Items.Item.InvertDirection(Item().Direction);
      _layout.GetCommandController().AddCommandAndExecute(new SetTrainDirectionCommand(_layout, this, invertedDirection));
    }

    #endregion

    #region Properties

    public string TrainName
    {
      get => Item().TrainName;
      set
      {
        Item().TrainName = value;
        NotifyPropertyChanged(nameof(TrainName));
      }
    }

    public bool ForceStop
    {
      get => Item().ForceStop;
      set
      {
        Item().ForceStop = value;
        NotifyPropertyChanged(nameof(ForceStop));
      }
    }

    public int Direction
    {
      get => Item().Direction;
      set
      {
        Item().Direction = value;
        NotifyPropertyChanged(nameof(Direction));
        NotifyPropertyChanged(nameof(Angle));
      }
    }

    public int Speed
    {
      get => Item().Speed;
      set
      {
        if (value < 1)
        {
          value = 1;
        }
        if (value > 150)
        {
          value = 150;
        }
        Item().Speed = value;
        NotifyPropertyChanged(nameof(Speed));
      }
    }

    public double Angle
    {
      get => -Direction * 45;
      private set { }
    }

    #endregion

  }
}
