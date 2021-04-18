using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Train2d.Main.Views
{
  /// <summary>
  /// Interaction logic for CommandsView.xaml
  /// </summary>
  public partial class CommandsView : UserControl
  {
    public CommandsView()
    {
      InitializeComponent();
    }


    public CommandController CommandController
    {
      get
      {
        return (CommandController)GetValue(CommandControllerProperty);
      }
      set
      {
        SetValue(CommandControllerProperty, value);
      }
    }

    public static readonly DependencyProperty CommandControllerProperty = DependencyProperty.Register(
      nameof(CommandController),
      typeof(CommandController),
      typeof(CommandsView),
      new PropertyMetadata(
        new CommandController(),
        OnCommandControllerChanged));

    public static void OnCommandControllerChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
    {
      var view = sender as CommandsView;
      if (view == null)
        return;

      // Dim group As TransformGroup = view.Settings.Group
      //view.ZoomContent.RenderTransform = view.Settings.Group;
      //view.ShowControls = view.Settings.ShowSettings;
      //view.ShowPositionY = view.Settings.ShowPositionY;
    }

  }
}
