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
    using System.Text;
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
            while (true)
            {
                try
                {
                    TcpClient client = _Server.AcceptTcpClient();
                    ServerClient serverClient = new ServerClient(client);
                    OnServerStatusChanged(ServerStatus.ClientConnected, serverClient);
                    Task.Run(() => ServerClientThread(serverClient));
                }
                catch (Exception ex)
                {
                    OnServerStatusChanged(ServerStatus.ServerStopped, null);
                }
            }
        }
        private void ServerClientThread(ServerClient serverClient)
        {
            OnServerStatusChanged(ServerStatus.ListeningForClient, serverClient);
            while (true)
            {
                try
                {
                    var str = serverClient.Reader.ReadString();
                    MessageBase msgObj = JsonConvert.DeserializeObject<MessageBase>(str);
                    OnMessageRecieved(serverClient, str, msgObj);
                }
                catch (Exception ex)
                {
                    OnServerStatusChanged(ServerStatus.ConnectionException, serverClient);
                    OnServerStatusChanged(ServerStatus.ClientDisconnected, serverClient);
                    break;
                }
            }
        }
        public void SendMessageToClient(ServerClient client, MessageBase msg)
        {
            try
            {
                string str = JsonConvert.SerializeObject(msg);
                client.Writer.Write(str);
                //client.Writer.Flush();
            }
            catch (Exception ex)
            {
                OnServerStatusChanged(ServerStatus.ConnectionException, client);
                OnServerStatusChanged(ServerStatus.ClientDisconnected, client);
            }
        }
        public void BroadcastToClients(MessageBase msg, ServerClient source = null)
        {
            if (source != null)
                (from x in _Clients
                 where x.ClientID != source.ClientID
                 select x).ToList().ForEach(x => SendMessageToClient(x, msg));
            else
                foreach (ServerClient client in _Clients)
                    SendMessageToClient(client, msg);
        }
        private void OnMessageRecieved(ServerClient client, string msgStr, MessageBase msgObj)
        {
            MessageRecieved?.Invoke(this, new MessageRevievedEventArgs(msgObj, client.Client.Client.Handle.ToInt32()));
            OnServerStatusChanged(ServerStatus.ProcessingIncommingMessage, client);
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
        private void OnServerStatusChanged(ServerStatus action, ServerClient serverClient)
        {
            switch (action)
            {
                case ServerStatus.ServerStopped:
                    foreach (ServerClient client in _Clients)
                        client.Dispose();
                    _Server.Stop();
                    break;
                case ServerStatus.ClientDisconnected:
                    serverClient.Dispose();
                    _Clients.Remove(serverClient);
                    break;
                case ServerStatus.ClientConnected:
                    _Clients.Add(serverClient);
                    break;
            }
            ServerStatusChanged?.Invoke(this, new ServerActionEventArgs(action, serverClient));
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
        public NetworkStream Stream { get; private set; }
        public BinaryReader Reader { get; private set; }
        public BinaryWriter Writer { get; private set; }
        public int ClientID => Client.Client.Handle.ToInt32();
        public string NickName { get; set; }
        public ServerClient(TcpClient client)
        {
            Client = client;
            Stream = client.GetStream();
            Reader = new BinaryReader(Stream);
            Writer = new BinaryWriter(Stream);
        }
        public override string ToString() =>
            string.Format("{0}: {1} => {1}", ClientID, Client.Client.LocalEndPoint, Client.Client.RemoteEndPoint);
        public void Dispose()
        {
            Reader.Dispose();
            Writer.Dispose();
            Stream.Dispose();
            Client.Dispose();
        }
    }
}

