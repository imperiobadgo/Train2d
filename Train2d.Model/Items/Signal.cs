using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train2d.Model.Items
{
  public class Signal : Item
  {

    /// <summary>
    /// 0: Hold
    /// 1: Go
    /// </summary>
    public int State { get; set; } = 0;
  }
}
