namespace ITI4InARow.Module.Core
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class MessageRevievedEventArgs
    {
        public MessageRevievedEventArgs(MessageBase msg, int clientHandle)
        {
            this.Message = msg;
            this.ClientHandle = clientHandle;
        }

        public int ClientHandle { get; private set; }

        public MessageBase Message { get; private set; }
    }
}

