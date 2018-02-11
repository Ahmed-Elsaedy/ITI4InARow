using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ITI4InARow.Module.Core;
using Newtonsoft.Json;
using System.Diagnostics;

namespace ITI4InARow.Module.Server
{
    public class ServerCore
    {
        TcpListener _Server;
        List<ServerClient> _Clients;
        public ServerCore(byte[] ipAddress, int port)
        {
            _Server = new TcpListener(new IPAddress(ipAddress), port);
            _Clients = new List<ServerClient>();
        }
        public void StartServer()
        {
            _Server.Start();
            OnServerStatusChanged(ServerStatus.ServerStarted, null);
            Task.Run(new Action(ServerMainThread));
        }
        private void ServerMainThread()
        {
            OnServerStatusChanged(ServerStatus.StartWaitingForClients, null);
            while (true)
            {
                if (_Server.Pending())
                {
                    OnServerStatusChanged(ServerStatus.IncommingClient, null);
                    TcpClient clientRequest = _Server.AcceptTcpClient();
                    ServerClient serverClient = new ServerClient(clientRequest);
                    OnServerStatusChanged(ServerStatus.ClientConnected, serverClient);
                    Task.Run(() => ServerClientThread(serverClient));
                }
            }
        }
        private void ServerClientThread(ServerClient serverClient)
        {
            OnServerStatusChanged(ServerStatus.ListeningForClient, serverClient);
            using (NetworkStream stream = serverClient.Client.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
                while (true)
                {
                    MonitorKeepALiveTime(serverClient);
                    try
                    {
                        if (stream.DataAvailable)
                        {
                            var clientStr = reader.ReadString();
                            MessageBase msgBase = JsonConvert.DeserializeObject<MessageBase>(clientStr);
                            ReadingMessageFlag(serverClient, clientStr, msgBase);
                        }
                        if (serverClient.Queue.Count > 0)
                        {
                            var msg = serverClient.Queue[0];
                            if (msg.Flag == MessageFlag.KeepAlive)
                                OnServerStatusChanged(ServerStatus.SendingKeepALiveFlag, serverClient);
                            else
                                OnServerStatusChanged(ServerStatus.SendingServerMessage, serverClient);

                            string resStr = JsonConvert.SerializeObject(msg);
                            serverClient.Queue.RemoveAt(0);
                            writer.Write(resStr);
                            writer.Flush();
                        }
                    }
                    catch (Exception ex)
                    {
                        OnServerStatusChanged(ServerStatus.ConnectionException, serverClient);
                        break;
                    }
                }
            OnServerStatusChanged(ServerStatus.ClientDisconnected, serverClient);
        }
        public void StopServer()
        {
            OnServerStatusChanged(ServerStatus.ServerStopped, null);
        }
        public void SendMessageToClient(ServerClient client, MessageBase msg)
        {
            client.Queue.Add(msg);
        }
        private void ReadingMessageFlag(ServerClient client, string msgStr, MessageBase msgObj)
        {
            if (msgObj.Flag == MessageFlag.KeepAlive)
                OnServerStatusChanged(ServerStatus.ReceivingKeepALiveFlag, client);
            else
            {
                object obj = JsonConvert.DeserializeObject(msgStr, msgObj.MsgType);
                OnMessageRecieved(client, msgObj);
            }
        }
        private void MonitorKeepALiveTime(ServerClient client)
        {
            var period = DateTime.Now - client.LastKeepALive;
            if (period.TotalSeconds > 2)
            {
                client.LastKeepALive = DateTime.Now;
                MessageBase keepAliveMsg = new MessageBase() { Flag = MessageFlag.KeepAlive };
                SendMessageToClient(client, keepAliveMsg);
            }
        }
        public event EventHandler<ServerActionEventArgs> ServerStatusChanged;
        public event EventHandler<MessageRevievedEventArgs> MessageRecieved;
        private void OnServerStatusChanged(ServerStatus action, ServerClient client)
        {
            switch (action)
            {
                case ServerStatus.ClientConnected:
                    _Clients.Add(client);
                    break;
                case ServerStatus.ServerStopped:
                    foreach (ServerClient sClient in _Clients)
                        client.Client.Close();
                    _Server.Stop();
                    break;
                case ServerStatus.ClientDisconnected:
                case ServerStatus.ConnectionException:
                    try { client.Client.Close(); }
                    catch { }
                    _Clients.Remove(client);
                    break;
            }
            ServerStatusChanged?.Invoke(this, new ServerActionEventArgs(action, client));
        }
        private void OnMessageRecieved(ServerClient client, MessageBase msgBase)
        {
            MessageRecieved?.Invoke(this, new MessageRevievedEventArgs(msgBase, client.Client.Client.Handle.ToInt32()));
            OnServerStatusChanged(ServerStatus.ProcessingIncommingMessage, client);
            ProcessClientMessage(client, msgBase);
        }
        protected ServerClient this[int handle]
        {
            get { return _Clients.SingleOrDefault(x => x.ClientID == handle); }
        }
        protected void ProcessClientMessage(ServerClient client, MessageBase msgBase)
        {
            switch (msgBase.MsgType.Name)
            {
                case "RegisterMessage":
                    OnRegisterMessage(client, (RegisterMessage)msgBase);
                    break;
            }
        }
        protected virtual void OnRegisterMessage(ServerClient client, RegisterMessage msg) { }
    }
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
        public override string ToString()
        {
            return $"{TimeStamp.ToLongTimeString()}: - {Status.ToString()}";
        }
    }
    public class ServerClient
    {
        public TcpClient Client { get; private set; }
        public int ClientID { get; private set; }
        public DateTime LastKeepALive { get; set; }
        public List<MessageBase> Queue { get; private set; }
        public ServerClient(TcpClient client)
        {
            Client = client;
            Queue = new List<MessageBase>();
            ClientID = client.Client.Handle.ToInt32();
            LastKeepALive = DateTime.Now;
        }
        public override string ToString()
        {
            return string.Format("{0}: {1} => {2}", ClientID, Client.Client.LocalEndPoint, Client.Client.RemoteEndPoint);
        }
    }
    public enum ServerStatus
    {
        ServerStarted,
        ClientConnected,
        ListeningForClient,
        ReadingClientStream,
        SendingServerMessage,
        ClientDisconnected,
        ServerStopCancelled,
        ServerStopped,
        PendingForClients,
        StopWaitingForClients,
        ConnectionException,
        ProcessingIncommingMessage,
        IncommingClient,
        StartWaitingForClients,
        ReceivingKeepALiveFlag,
        SendingKeepALiveFlag
    }
}