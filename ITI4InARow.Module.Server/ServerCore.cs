namespace ITI4InARow.Module.Server
{
    using ITI4InARow.Module.Core;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;

    public class ServerCore
    {
        private List<ServerClient> _Clients;
        private TcpListener _Server;

        public event EventHandler<MessageRevievedEventArgs> MessageRecieved;
        public event EventHandler<ServerActionEventArgs> ServerStatusChanged;

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
                foreach (ServerClient client in _Clients)
                {
                    MessageBase msg = new MessageBase
                    {
                        Flag = MessageFlag.KeepAlive
                    };
                    SendMessageToClient(client, msg);
                }
                if (_Server.Pending())
                {
                    OnServerStatusChanged(ServerStatus.IncommingClient, null);
                    TcpClient client2 = _Server.AcceptTcpClient();
                    ServerClient serverClient = new ServerClient(client2);
                    OnServerStatusChanged(ServerStatus.ClientConnected, serverClient);
                    Task.Run((Action)(() => ServerClientThread(serverClient)));
                }
                Thread.Sleep(0x7d0);
            }
        }
        private void ServerClientThread(ServerClient serverClient)
        {
            OnServerStatusChanged(ServerStatus.ListeningForClient, serverClient);
            using (NetworkStream stream = serverClient.Client.GetStream())
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        bool flag4;
                        goto Label_00D6;
                        Label_0029:;
                        try
                        {
                            if (stream.DataAvailable)
                            {
                                string str = reader.ReadString();
                                MessageBase msgObj = JsonConvert.DeserializeObject<MessageBase>(str);
                                ReadingMessageFlag(serverClient, str, msgObj);
                            }
                            if (serverClient.Queue.Count > 0)
                            {
                                MessageBase base3 = serverClient.Queue[0];
                                if (base3.Flag == MessageFlag.KeepAlive)
                                {
                                    //OnServerStatusChanged(ServerStatus.SendingKeepALiveFlag, serverClient);
                                }
                                else
                                {
                                    OnServerStatusChanged(ServerStatus.SendingServerMessage, serverClient);
                                }
                                string str2 = JsonConvert.SerializeObject(base3);
                                serverClient.Queue.RemoveAt(0);
                                writer.Write(str2);
                                writer.Flush();
                            }
                        }
                        catch (Exception)
                        {
                            OnServerStatusChanged(ServerStatus.ConnectionException, serverClient);
                            goto Label_0105;
                        }
                        Label_00D6:
                        flag4 = true;
                        goto Label_0029;
                    }
                }
            }
            Label_0105:
            OnServerStatusChanged(ServerStatus.ClientDisconnected, serverClient);
        }
        public void SendMessageToClient(ServerClient client, MessageBase msg)
        {
            client.Queue.Add(msg);
        }
        public void BroadcastToClients(MessageBase msg, ServerClient source = null)
        {
            if (source != null)
                (from x in _Clients
                 where x.ClientID != source.ClientID
                    select x).ToList().ForEach(x => x.Queue.Add(msg));
            else
                foreach (ServerClient client in _Clients)
                    client.Queue.Add(msg);
        }
        private void MonitorKeepALiveTime(ServerClient client)
        {
            TimeSpan span = (TimeSpan) (DateTime.Now - client.LastKeepALive);
            if (span.TotalSeconds > 2.0)
            {
                client.LastKeepALive = DateTime.Now;
                MessageBase msg = new MessageBase {
                    Flag = MessageFlag.KeepAlive
                };
                SendMessageToClient(client, msg);
            }
        }
        private void ReadingMessageFlag(ServerClient client, string msgStr, MessageBase msgObj)
        {
            if (msgObj.Flag == MessageFlag.KeepAlive)
                OnServerStatusChanged(ServerStatus.ReceivingKeepALiveFlag, client);
            else
                OnMessageRecieved(client, msgStr, msgObj);
        }
        private void OnMessageRecieved(ServerClient client, string msgStr, MessageBase msgObj)
        {
            MessageRecieved?.Invoke(this, new MessageRevievedEventArgs(msgObj, client.Client.Client.Handle.ToInt32()));
            OnServerStatusChanged(ServerStatus.ProcessingIncommingMessage, client);
            ProcessClientMessage(client, msgStr, msgObj);
        }
        protected void ProcessClientMessage(ServerClient client, string msgStr, MessageBase msgObj)
        {
            switch (msgObj.MsgType)
            {
                case MessageType.ProfileUpdateMessage:
                    OnProfileUpdateMessage(client, JsonConvert.DeserializeObject<ProfileUpdateMessage>(msgStr));
                    break;

                case MessageType.RoomUpdateMessage:
                    OnRoomUpdateMessage(client, JsonConvert.DeserializeObject<RoomUpdateMessage>(msgStr));
                    break;

                case MessageType.GameUpdateMessage:
                    OnGameUpdateMessage(client, JsonConvert.DeserializeObject<GameUpdateMessage>(msgStr));
                    break;
            }
        }
        private void OnServerStatusChanged(ServerStatus action, ServerClient client)
        {
            switch (action)
            {
                case ServerStatus.ServerStopped:
                    foreach (ServerClient client2 in _Clients)
                        client.Client.Close();
                    _Server.Stop();
                    break;
                case ServerStatus.ConnectionException:
                case ServerStatus.ClientDisconnected:
                    try { client.Client.Close(); }
                    catch { }
                    _Clients.Remove(client);
                    break;
                case ServerStatus.ClientConnected:
                    _Clients.Add(client);
                    break;
            }
            ServerStatusChanged?.Invoke(this, new ServerActionEventArgs(action, client));
        }
        public void StopServer()
        {
            OnServerStatusChanged(ServerStatus.ServerStopped, null);
        }
        protected virtual void OnGameUpdateMessage(ServerClient client, GameUpdateMessage msg)
        {
        }
        protected virtual void OnProfileUpdateMessage(ServerClient client, ProfileUpdateMessage msg)
        {
        }
        protected virtual void OnRoomUpdateMessage(ServerClient client, RoomUpdateMessage msg)
        {
        }
        protected ServerClient this[int handle] =>
            _Clients.SingleOrDefault<ServerClient>(x => (x.ClientID == handle));
    }

    public class ServerClient
    {
        public TcpClient Client { get; private set; }
        public int ClientID { get; private set; }
        public string ClientIP { get; set; }
        public DateTime LastKeepALive { get; set; }
        public string NickName { get; set; }
        public List<MessageBase> Queue { get; private set; }
        public ServerClient(TcpClient client)
        {
            Client = client;
            Queue = new List<MessageBase>();
            ClientID = client.Client.Handle.ToInt32();
            LastKeepALive = DateTime.Now;
        }
        public override string ToString() =>
            string.Format("{0}: {1} => {1}", ClientID, Client.Client.LocalEndPoint, Client.Client.RemoteEndPoint);
    }
}

