using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI4InARow.Module.Core
{
    public class MessageBase
    {
        public MessageType MsgType { get; set; }
    }
    public enum MessageType
    {
        RegisterMsg
    }
}
