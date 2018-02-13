namespace ITI4InARow.Module.Server
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class ServerActionEventArgs
    {
        public ServerStatus Status { get; private set; }
        public ServerClient Client { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public ServerActionEventArgs(ServerStatus status, ServerClient client)
        {
            Status = status;
            Client = client;
            TimeStamp = DateTime.Now;
        }
        public override string ToString() =>
            $"{TimeStamp.ToLongTimeString()}: - {Status.ToString()}";
    }
}

