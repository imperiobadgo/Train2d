using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Train2d.Model.Items;

namespace Train2d.Main.ViewModel.Items
{
  public class TrainViewModel : ItemGenericViewModel<Train>
  {
    #region Construct

    public TrainViewModel()
    {
      SetItem(new Train());
    }

    public TrainViewModel(Train newTrain)
    {
      SetItem(newTrain);
    }

    #endregion
  }
}
