using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Train2d.Model.Items;

namespace Train2d.Main.ViewModel.Items
{
  public class TrainViewModel : ItemGenericViewModel<Train>
  {
    #region Construct

    public TrainViewModel() : this(new Train())
    { }

    public TrainViewModel(Train newTrain)
    {
      SetItem(newTrain);
      DisplayOrder = 5;
      MainColor = Brushes.DarkBlue;
    }

    #endregion
  }
}
