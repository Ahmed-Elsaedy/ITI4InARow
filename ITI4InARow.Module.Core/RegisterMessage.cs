using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI4InARow.Module.Core
{
    public class RegisterMessage : MessageBase
    {
        public int Id { get; set; }
        public string Color { get; set; }
        public string IP { get; set; }
        public string Name { get; set; }
    }

}
