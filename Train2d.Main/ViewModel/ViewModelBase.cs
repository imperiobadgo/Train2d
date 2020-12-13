using System.ComponentModel;

namespace Train2d.Main.ViewModel
{
  class ViewModelBase : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;
  }
}
