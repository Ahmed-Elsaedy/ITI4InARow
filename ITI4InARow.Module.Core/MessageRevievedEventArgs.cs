using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI4InARow.Module.Core
{
    public class MessageRevievedEventArgs
    {
        public MessageBase Message { get; private set; }
        public int ClientHandle { get; private set; }
        public MessageRevievedEventArgs(MessageBase msg, int clientHandle)
        {
            Message = msg;
            ClientHandle = clientHandle;
        }
    }
}
