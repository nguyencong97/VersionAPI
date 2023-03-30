using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL.DTO
{
  public  class Login
    {
        public string role;
        public string userID;
        public string accessToken;
        public IEnumerable<ComboboxDTO> apps;
    }
}
