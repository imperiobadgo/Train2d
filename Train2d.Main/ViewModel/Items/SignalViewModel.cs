using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Train2d.Model.Items;

namespace Train2d.Main.ViewModel.Items
{
  public class SignalViewModel : ItemGenericViewModel<Signal>
  {
    #region Construct

    public SignalViewModel() : this(new Signal())
    {}

    public SignalViewModel(Signal newSignal)
    {
      SetItem(newSignal);
      DisplayOrder = 3;
      MainColor = Brushes.Green;
    }

    protected override void OnItemSet(Item item)
    {

    }

    #endregion
  }
}
