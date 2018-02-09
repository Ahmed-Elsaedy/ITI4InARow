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
    public class ClientCore : IDisposable
    {
        private TcpClient _Client;
        private NetworkStream _RStream = null;
        private BinaryReader _Reader = null;
        private BinaryWriter _Writer = null;
        private List<MessageBase> _Queue = null;
        public bool ConnectClient(byte[] ipAddress, int port)
        {
            bool retVal = false;
            _Client = new TcpClient();
            _Queue = new List<MessageBase>();
            try
            {
                _Client.Connect(new IPAddress(ipAddress), port);
                OnClientStatusChanged(ClientStatus.ClientConnected);
                CreateTaskForServer(_Client);
                retVal = true;
            }
            catch (SocketException)
            { OnClientStatusChanged(ClientStatus.ConnectionError); }
            return retVal;
        }
        public void SendMessageToServer(MessageBase message)
        {
            _Queue.Add(message);
        }
        private void CreateTaskForServer(TcpClient request)
        {
            Task.Run(() =>
            {
                OnClientStatusChanged(ClientStatus.ListeningForServer);
                while (request.Connected)
                {
                    try
                    {
                        _RStream = request.GetStream();
                        if (_RStream.DataAvailable)
                        {
                            OnClientStatusChanged(ClientStatus.ReadingServerStream);
                            _Reader = new BinaryReader(_RStream);
                            var serverStr = _Reader.ReadString();
                            MessageBase msgBase = JsonConvert.DeserializeObject<MessageBase>(serverStr);
                            object obj = JsonConvert.DeserializeObject(serverStr, msgBase.MsgType);
                            OnMessageRecieved(msgBase);
                        }
                        if (_Queue.Count > 0)
                        {
                            OnClientStatusChanged(ClientStatus.SendingClientMessage);
                            string resStr = JsonConvert.SerializeObject(_Queue[0]);
                            _Queue.RemoveAt(0);
                            _Writer = new BinaryWriter(_RStream);
                            _Writer.Write(resStr);
                            _Writer.Flush();
                        }
                    }
                    catch (IOException)
                    {
                        OnClientStatusChanged(ClientStatus.ClientDisconnectedError);
                        break;
                    }
                }
                OnClientStatusChanged(ClientStatus.ClientDisconnected);
            });
        }
        protected virtual void ProcessServerMessage(MessageBase msgBase)
        {

        }
        public void DisconnectClient()
        {
            OnClientStatusChanged(ClientStatus.ClientDisconnected);
        }
        public event EventHandler<ClientActionEventArgs> ClientStatusChanged;
        public event EventHandler<MessageRevievedEventArgs> MessageRecieved;
        private void OnClientStatusChanged(ClientStatus status)
        {
            switch (status)
            {
                case ClientStatus.ClientDisconnected:
                case ClientStatus.ClientDisconnectedError:
                    Dispose();
                    break;
            }
            ClientStatusChanged?.Invoke(this, new ClientActionEventArgs(status));
        }
        private void OnMessageRecieved(MessageBase msgBase)
        {
            MessageRecieved?.Invoke(this, new MessageRevievedEventArgs(msgBase, msgBase.ClientID));
            OnClientStatusChanged(ClientStatus.ProcessingServerMessage);
            ProcessServerMessage(msgBase);
        }
        public void Dispose()
        {
            if (_Client != null)
            {
                _Reader.Dispose();
                _Writer.Dispose();
                _RStream.Dispose();
                _Client.Dispose();
            }
        }
        ~ClientCore()
        {
            try { Dispose(); }
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
        ReadingServerStream,
        SendingClientMessage,
        ClientDisconnectedError,
        ProcessingServerMessage,
        ListeningForServer
    }
}
