using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Train2d.Main.ViewModel.Items
{
  interface IUpdateableItem
  {
    void Update(LayoutViewModel layout, float deltaTime);
  }
}
