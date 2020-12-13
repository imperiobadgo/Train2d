using System;
using System.Collections.Generic;

namespace Train2d.Main.ViewModel
{
  class MainWindowViewModel : ViewModelBase
  {

    public MainWindowViewModel()
    {
      Layouts = new List<LayoutViewModel>();
      Layouts.Add(new LayoutViewModel());
      SelectedView = Layouts[0];
    }

    public List<LayoutViewModel> Layouts;

    public LayoutViewModel SelectedView { get; set; }

  }


}
