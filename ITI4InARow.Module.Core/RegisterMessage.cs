﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI4InARow.Module.Core
{
    public class RegisterMessage : MessageBase
    {
        public string Name { get; set; }
        public string ipaddress { get; set; }

    }
}
