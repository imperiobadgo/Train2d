using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train2d.Model.Items
{
  public class Signal : Item
  {
    #region constants

    public const int STATE_HOlD = 0;
    public const int STATE_GO = 1;

    #endregion


    /// <summary>
    /// 0: Hold
    /// 1: Go
    /// </summary>
    public int State { get; set; } = 0;


    public int Direction { get; set; } = 0;
  }
}
