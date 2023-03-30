using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.DTO
{
  public  class MenuDTO
    {
        public ComboboxDTO menu;
        public IEnumerable<ComboboxDTO> subMenus;
    }
}
