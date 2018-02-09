using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI4InARow.Module.Core
{
    public class MessageBase
    {
        public int Flag { get; set; }
        public Type MsgType { get; set; }
        public int ClientID { get; set; }
        public MessageBase()
        {
            MsgType = GetType();
        }
    }
}
