using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Train2d.Main.Views
{
  public class CommandViewSelector : DataTemplateSelector
  {
    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
      FrameworkElement element = container as FrameworkElement;

      var command = item as Commands.CommandBase;
      var chain = item as Commands.CommandChain;
      if (command == null)
        return null;
      if (chain != null)
        return
            element.FindResource("CommandChainDataTemplate")
            as DataTemplate;
      else
        return
            element.FindResource("BaseCommandDataTemplate")
            as DataTemplate;
    }
  }
}
