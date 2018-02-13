namespace ITI4InARow.Module.Client
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class ClientActionEventArgs
    {
        public ClientStatus Status { get; private set; }
        public ClientActionEventArgs(ClientStatus status)
        {
            Status = status;
        }
    }
}

