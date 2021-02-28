using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Train2d.Model.Items;

namespace Train2d.Main.ViewModel.Items
{
  public class SignalViewModel : ItemGenericViewModel<Signal>
  {
    #region Construct

    public SignalViewModel()
    {
      SetItem(new Signal());
    }

    public SignalViewModel(Signal newSignal)
    {
      SetItem(newSignal);
    }

    #endregion
  }
}
