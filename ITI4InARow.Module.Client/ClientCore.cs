using ITI4InARow.Module.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ITI4InARow.Module.Client
{
    public class ClientCore
    {
        TcpClient _Client;
        public List<MessageBase> Queue { get; private set; }
        public void ConnectClient(byte[] ipAddress, int port)
        {
            _Client = new TcpClient();
            Queue = new List<MessageBase>();
            try
            {
                _Client.Connect("RedHawk", port);
                OnClientStatusChanged(ClientStatus.ClientConnected);
                CreateTaskForServer(_Client);
            }
            catch (SocketException)
            { OnClientStatusChanged(ClientStatus.ConnectionError); }
        }
        private void CreateTaskForServer(TcpClient request)
        {
            Task.Run(() =>
            {
                while (request.Connected)
                {
                    OnClientStatusChanged(ClientStatus.ListeningForServer);
                    try
                    {
                        NetworkStream _RStream = request.GetStream();

                        // Reading Bytes From Server
                        byte[] data = new byte[request.ReceiveBufferSize];
                        _RStream.Read(data, 0, request.ReceiveBufferSize);
                        OnClientStatusChanged(ClientStatus.ReadServerQueue);

                        // Converting Bytes To List Of Messages
                        var serverStr = Encoding.Default.GetString(data);
                        List<MessageBase> serverQueue = JsonConvert.DeserializeObject<List<MessageBase>>(serverStr);
                        OnClientStatusChanged(ClientStatus.ProcessingServerMessages);
                        ProcessServerMessages(serverQueue);

                        // Serializing Current Queue and Clear after that
                        string queueStr = JsonConvert.SerializeObject(Queue);
                        Queue.Clear();

                        // Writing Current Queue To Stream
                        byte[] queueBytes = Encoding.Default.GetBytes(queueStr);
                        _RStream.Write(queueBytes, 0, queueBytes.Length);
                        OnClientStatusChanged(ClientStatus.SendClientQueue);
                        _RStream.Flush();
                        _RStream.Close();
                    }
                    catch (IOException)
                    {
                        request.Close();
                        OnClientStatusChanged(ClientStatus.ClientDisconnectedError);
                        break;
                    }
                }
                OnClientStatusChanged(ClientStatus.ClientDisconnected);
            });
        }
        protected virtual void ProcessServerMessages(List<MessageBase> serverQueue)
        {

        }
        public void DisconnectClient()
        {
            _Client.Close();
            OnClientStatusChanged(ClientStatus.ClientDisconnected);
        }
        public event EventHandler<ClientActionEventArgs> ClientStatusChanged;
        private void OnClientStatusChanged(ClientStatus status)
        {
            ClientStatusChanged?.Invoke(this, new ClientActionEventArgs(status));
        }
        ~ClientCore()
        {
            try { _Client.Close(); }
            catch { }
        }
    }

    public class ClientActionEventArgs
    {
        public ClientStatus Status { get; private set; }
        public ClientActionEventArgs(ClientStatus status)
        {
            Status = status;
        }
    }
    public enum ClientStatus
    {
        ClientConnected,
        ClientDisconnected,
        ConnectionError,
        ListeningForServer,
        ReadServerQueue,
        SendClientQueue,
        ClientDisconnectedError,
        ProcessingServerMessages
    }
}
