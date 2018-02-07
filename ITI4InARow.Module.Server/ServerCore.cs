using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

namespace ITI4InARow.Module.Server
{
    public class ServerCore
    {
        TcpListener _Server;
        List<TcpClient> _Clients;
        public ServerCore(byte[] ipAddress, int port)
        {
            _Server = new TcpListener(new IPAddress(ipAddress), port);
            _Clients = new List<TcpClient>();
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
                        byte[] data = new byte[request.ReceiveBufferSize];
                        _RStream.Read(data, 0, request.ReceiveBufferSize);
                        OnServerStatusChanged(ServerStatus.ReadClientRequest, request);
                        // ReadMessage(data); deserlize for object of type Message
                        byte[] response = data;
                        _RStream.Write(response, 0, response.Length);
                        _RStream.Flush();
                        OnServerStatusChanged(ServerStatus.WriteServerResponse, request);
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

        public event EventHandler<ServerActionEventArgs> ServerStatusChanged;
        private void OnServerStatusChanged(ServerStatus action, TcpClient client)
        {
            ServerClient sClient = null;
            if (client != null)
            {
                sClient = new ServerClient(client.Client.Handle);
                if (client.Connected)
                {
                    sClient.LocalEndPoint = client.Client.LocalEndPoint;
                    sClient.RemoteEndPoint = client.Client.RemoteEndPoint;
                }
            }
            switch (action)
            {
                case ServerStatus.ClientConnected:
                    _Clients.Add(client);
                    break;
                case ServerStatus.ClientDisconnected:
                    _Clients.Remove(client);
                    break;
            }
            ServerStatusChanged?.Invoke(this, new ServerActionEventArgs(action, sClient));
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
        public IntPtr Handle { get; private set; }
        public EndPoint LocalEndPoint { get; internal set; }
        public EndPoint RemoteEndPoint { get; internal set; }

        public ServerClient(IntPtr handle)
        {
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
        ForcedClientClose
    }
}