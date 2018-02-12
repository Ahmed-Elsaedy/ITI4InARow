namespace ITI4InARow.Module.Server
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class ServerActionEventArgs
    {
       public ServerActionEventArgs(ServerStatus status, ServerClient client)
        {
            this.Status = status;
            this.Client = client;
            this.TimeStamp = DateTime.Now;
        }

        public override string ToString() => 
            $"{this.TimeStamp.ToLongTimeString()}: - {this.Status.ToString()}";

        public ServerClient Client { get; private set; }

        public ServerStatus Status { get; private set; }

        public DateTime TimeStamp { get; private set; }
    }
}

