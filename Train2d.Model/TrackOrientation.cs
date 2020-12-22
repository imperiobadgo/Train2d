using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train2d.Model
{
  public enum TrackOrientation
  {
    Horizontal,
    Vertical,
    Diagonal,//northwest -> southeast
    AntiDiagonal//southwest -> northeast
  }
}
