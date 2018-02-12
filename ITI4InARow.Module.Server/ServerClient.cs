namespace ITI4InARow.Module.Server
{
    using ITI4InARow.Module.Core;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;

    public class ServerClient
    {
        public ServerClient(TcpClient client)
        {
            this.Client = client;
            this.Queue = new List<MessageBase>();
            this.ClientID = client.Client.Handle.ToInt32();
            this.LastKeepALive = DateTime.Now;
        }

        public override string ToString() => 
            string.Format("{0}: {1} => {1}", this.ClientID, this.Client.Client.LocalEndPoint, this.Client.Client.RemoteEndPoint);

        public TcpClient Client { get; private set; }

        public int ClientID { get; private set; }

        public string ClientIP { get; set; }

        public DateTime LastKeepALive { get; set; }

        public string NickName { get; set; }

        public List<MessageBase> Queue { get; private set; }
    }
}

