using System;
using System.Collections.Generic;
using Train2d.Model;

namespace Train2d.Main.ViewModel
{
  public class MainWindowViewModel : ViewModelBase
  {

    public MainWindowViewModel()
    {
      CommandController = new CommandController();
      Layouts = new List<LayoutViewModel>();
      Layouts.Add(new LayoutViewModel(this));
      SelectedView = Layouts[0];
    }

    public List<LayoutViewModel> Layouts;

    public LayoutViewModel SelectedView { get; set; }

    public CommandController CommandController { get; private set; }

  }


}
