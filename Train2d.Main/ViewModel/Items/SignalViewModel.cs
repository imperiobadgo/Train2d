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
  public class SignalViewModel : ItemGenericViewModel<Signal>
  {
    #region Construct

    public SignalViewModel() : this(new Signal())
    { }

    public SignalViewModel(Signal newSignal)
    {
      SetItem(newSignal);
      DisplayOrder = 3;
      MainColor = Brushes.Red;
    }

    protected override void OnItemSet(Item item)
    {

    }

    #endregion

    #region Methods

    public void SetDirection(int newDirection)
    {
      Direction = newDirection;
    }

    public void SetState(int newState)
    {
      State = newState;
    }

    public override void OnSelectMain(LayoutController controller, LayoutViewModel layout)
    {
      int currentState = Item().State;
      currentState += 1;
      if (currentState == 2)
      {
        currentState = 0;
      }
      layout.GetCommandController().AddCommand(new SetSignalStateCommand(layout, this, currentState));
    }

    #endregion


    #region Properties

    public int State
    {
      get => Item().State;
      private set
      {
        Item().State = value;
        if (value == 0)
        {
          MainColor = Brushes.Red;
        }
        else if (value == 1)
        {
          MainColor = Brushes.Green;
        }
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

    public double Angle
    {
      get
      {
        return -Direction * 45;
      }
      private set { }
    }

    #endregion

  }
}
