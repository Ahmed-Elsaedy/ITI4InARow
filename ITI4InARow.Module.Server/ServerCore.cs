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
    public class ServerCore
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
                OnServerStatusChanged(ServerStatus.StartWaitingForClients, null);
                try
                {
                    TcpClient clientRequest = await _Server.AcceptTcpClientAsync();
                    OnServerStatusChanged(ServerStatus.ClientConnected, clientRequest);
                    CreateTaskForClient(clientRequest);
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
                _Server.Stop();
                OnServerStatusChanged(ServerStatus.ServerStopped, null);
                return true;
            }
        }
        private void CreateTaskForClient(TcpClient request)
        {
            Task.Run(() =>
            {
                while (request.Connected)
                {
                    OnServerStatusChanged(ServerStatus.ListeningForClient, request);
                    try
                    {
                        NetworkStream _RStream = request.GetStream();
                        // Reading Bytes From Client
                        byte[] data = new byte[request.ReceiveBufferSize];
                        _RStream.Read(data, 0, request.ReceiveBufferSize);
                        OnServerStatusChanged(ServerStatus.ReadClientRequest, request);

                        // Converting Bytes To List Of Messages
                        var clientStr = Encoding.Default.GetString(data);
                        List<MessageBase> clientQueue = JsonConvert.DeserializeObject<List<MessageBase>>(clientStr);

                        OnServerStatusChanged(ServerStatus.ProcessingClientMessages, request);
                        ProcessClientMessages(clientQueue);

                        // Serializing Current Client Queue and Clear after that
                        string queueStr = JsonConvert.SerializeObject(this[request.Client.Handle].Queue);
                        this[request.Client.Handle].Queue.Clear();

                        // Writing Current Queue To Stream
                        byte[] queueBytes = Encoding.Default.GetBytes(queueStr);
                        _RStream.Write(queueBytes, 0, queueBytes.Length);
                        OnServerStatusChanged(ServerStatus.WriteServerResponse, request);
                        _RStream.Flush();
                        _RStream.Close();
                    }
                    catch (IOException)
                    {
                        request.Close();
                        OnServerStatusChanged(ServerStatus.ForcedClientClose, request);
                        break;
                    }
                }
                OnServerStatusChanged(ServerStatus.ClientDisconnected, request);
            });
        }
        protected virtual void ProcessClientMessages(List<MessageBase> serverQueue)
        {

        }
        public event EventHandler<ServerActionEventArgs> ServerStatusChanged;
        private void OnServerStatusChanged(ServerStatus action, TcpClient client)
        {
            ServerClient serverClient = null;
            if (client != null)
                serverClient = this[client.Client.Handle];
            switch (action)
            {
                case ServerStatus.ClientConnected:
                    ServerClient sClient = new ServerClient(client, client.Client.Handle)
                    {
                        LocalEndPoint = client.Client.LocalEndPoint,
                        RemoteEndPoint = client.Client.RemoteEndPoint
                    };
                    break;
                case ServerStatus.ClientDisconnected:
                    if (serverClient != null)
                        _Clients.Remove(serverClient);
                    break;
            }
            ServerStatusChanged?.Invoke(this, new ServerActionEventArgs(action, serverClient));
        }
        private ServerClient this[IntPtr handle]
        {
            get { return _Clients.SingleOrDefault(x => x.Handle == handle); }
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
    public class ServerClient
    {
        private TcpClient client;
        public IntPtr Handle { get; private set; }
        public EndPoint LocalEndPoint { get; set; }
        public EndPoint RemoteEndPoint { get; set; }
        public List<MessageBase> Queue { get; private set; }

        public ServerClient(TcpClient client, IntPtr handle)
        {
            this.client = client;
            Queue = new List<MessageBase>();
            Handle = handle;
        }
        public override string ToString()
        {
            return string.Format("{0}: {1} => {1}", Handle, LocalEndPoint, RemoteEndPoint);
        }
    }
    public enum ServerStatus
    {
        ServerStarted,
        ClientConnected,
        ListeningForClient,
        ReadClientRequest,
        WriteServerResponse,
        ClientDisconnected,
        ServerStopCancelled,
        ServerStopped,
        StartWaitingForClients,
        StopWaitingForClients,
        ForcedClientClose,
        ProcessingClientMessages
    }
}