using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Train2d.Model.Items;

namespace Train2d.Main.ViewModel.Items
{
  public class TrackSwitchViewModel : ItemGenericViewModel<TrackSwitch>
  {
    #region Construct

    public TrackSwitchViewModel()
    {
      SetItem(new TrackSwitch());
    }

    public TrackSwitchViewModel(TrackSwitch newSwitch)
    {
      SetItem(newSwitch);
    }

    #endregion
  }
}
