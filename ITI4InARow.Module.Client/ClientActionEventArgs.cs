namespace ITI4InARow.Module.Client
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class ClientActionEventArgs
    {
        public ClientActionEventArgs(ClientStatus status)
        {
            this.Status = status;
        }

        public ClientStatus Status { get; private set; }
    }
}

