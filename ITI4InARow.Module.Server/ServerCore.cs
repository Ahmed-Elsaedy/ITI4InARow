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

namespace ITI4InARow.Module.Server
{
    public class ServerCore : IDisposable
    {
        TcpListener _Server;
        List<ServerClient> _Clients;
        public ServerCore(byte[] ipAddress, int port)
        {
            _Server = new TcpListener(new IPAddress(ipAddress), port);
            _Clients = new List<ServerClient>();
        }
        public async void StartServerAsync()
        {
            _Server.Start();
            OnServerStatusChanged(ServerStatus.ServerStarted, null);
            while (true)
            {
                OnServerStatusChanged(ServerStatus.WaitingForClients, null);
                try
                {
                    TcpClient clientRequest = await _Server.AcceptTcpClientAsync();
                    var serverClient = new ServerClient(clientRequest);
                    OnServerStatusChanged(ServerStatus.ClientConnected, serverClient);
                    CreateTaskForClient(serverClient);
                }
                catch (ObjectDisposedException)
                {
                    OnServerStatusChanged(ServerStatus.StopWaitingForClients, null);
                    break;
                }
            }
        }
        public bool StopServer()
        {
            if (_Clients.Count > 0)
            {
                OnServerStatusChanged(ServerStatus.ServerStopCancelled, null);
                return false;
            }
            else
            {
                OnServerStatusChanged(ServerStatus.ServerStopped, null);
                return true;
            }
        }
        private void CreateTaskForClient(ServerClient request)
        {
            Task.Run(() =>
            {
                OnServerStatusChanged(ServerStatus.ListeningForClient, request);
                while (request.Client.Connected)
                {
                    try
                    {
                        request.Stream = request.Client.GetStream();
                        if (request.Stream.DataAvailable)
                        {
                            OnServerStatusChanged(ServerStatus.ReadingClientStream, request);
                            request.Reader = new BinaryReader(request.Stream);
                            var clientStr = request.Reader.ReadString();
                            MessageBase msgBase = JsonConvert.DeserializeObject<MessageBase>(clientStr);
                            object obj = JsonConvert.DeserializeObject(clientStr, msgBase.MsgType);
                            OnMessageRecieved(request, msgBase);
                        }
                        if (request.Queue.Count > 0)
                        {
                            OnServerStatusChanged(ServerStatus.SendingServerMessage, request);
                            string resStr = JsonConvert.SerializeObject(request.Queue[0]);
                            request.Queue.RemoveAt(0);
                            request.Writer = new BinaryWriter(request.Stream);
                            request.Writer.Write(resStr);
                            request.Writer.Flush();
                        }
                    }
                    catch (IOException)
                    {
                        OnServerStatusChanged(ServerStatus.ForcedClientClose, request);
                        break;
                    }
                }
                OnServerStatusChanged(ServerStatus.ClientDisconnected, request);
            });
        }
        public void SendMessageToClient(ServerClient client, MessageBase msg)
        {
            client.Queue.Add(msg);
        }
        protected virtual void ProcessClientMessage(ServerClient client, MessageBase msgBase)
        {

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
                    Dispose();
                    break;
                case ServerStatus.ClientDisconnected:
                case ServerStatus.ForcedClientClose:
                    client.Dispose();
                    _Clients.Remove(client);
                    break;
            }
            ServerStatusChanged?.Invoke(this, new ServerActionEventArgs(action, client));
        }
        private void OnMessageRecieved(ServerClient client, MessageBase msgBase)
        {
            MessageRecieved?.Invoke(this, new MessageRevievedEventArgs(msgBase, client.Client.Client.Handle.ToInt32()));
            OnServerStatusChanged(ServerStatus.ProcessingClientMessage, client);
            ProcessClientMessage(client, msgBase);
        }
        public void Dispose()
        {
            foreach (ServerClient client in _Clients)
                client.Dispose();
            _Server.Stop();
        }
        ~ServerCore()
        {
            try { Dispose(); }
            catch { }
        }
        private ServerClient this[int handle]
        {
            get { return _Clients.SingleOrDefault(x => x.ClientID == handle); }
        }
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
            return $"{TimeStamp.ToLongTimeString()}: {Status.ToString()}";
        }
    }
    public class ServerClient : IDisposable
    {
        public NetworkStream Stream { get; set; }
        public BinaryReader Reader { get; set; }
        public BinaryWriter Writer { get; set; }
        public TcpClient Client { get; private set; }
        public int ClientID { get; private set; }
        public List<MessageBase> Queue { get; private set; }
        public ServerClient(TcpClient client)
        {
            Client = client;
            Queue = new List<MessageBase>();
            ClientID = client.Client.Handle.ToInt32();
        }
        public override string ToString()
        {
            return string.Format("{0}: {1} => {1}", ClientID, Client.Client.LocalEndPoint, Client.Client.RemoteEndPoint);
        }
        public void Dispose()
        {
            if (Writer != null) Writer.Dispose();
            if (Reader != null) Reader.Dispose();
            if (Stream != null) Stream.Dispose();
            Client.Dispose();
        }
        ~ServerClient()
        {
            try { Dispose(); }
            catch { }
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
        WaitingForClients,
        StopWaitingForClients,
        ForcedClientClose,
        ProcessingClientMessage
    }
}