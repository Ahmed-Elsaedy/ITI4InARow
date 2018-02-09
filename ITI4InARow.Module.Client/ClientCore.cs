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
        private TcpClient _Client;
        private bool _IsConnected;
        private NetworkStream _RStream = null;
        private BinaryReader _Reader = null;
        private BinaryWriter _Writer = null;
        private List<MessageBase> _Queue = null;
        public void ConnectClient(byte[] ipAddress, int port)
        {
            _Client = new TcpClient();
            _Queue = new List<MessageBase>();
            try
            {
                _Client.Connect(new IPAddress(ipAddress), port);
                OnClientStatusChanged(ClientStatus.ClientConnected);
                CreateTaskForServer(_Client);
            }
            catch (SocketException)
            { OnClientStatusChanged(ClientStatus.ConnectionError); }
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
                _RStream = request.GetStream();
                while (_IsConnected)
                {
                    if (_RStream.DataAvailable)
                    {
                        OnClientStatusChanged(ClientStatus.ReadingServerStream);
                        _Reader = new BinaryReader(_RStream);
                        var serverStr = _Reader.ReadString();
                        MessageBase msgBase = JsonConvert.DeserializeObject<MessageBase>(serverStr);
                        PreProcessServerMessage(serverStr, msgBase);
                    }
                    if (_Queue.Count > 0)
                    {
                        OnClientStatusChanged(ClientStatus.SendingClientMessage);
                        try
                        {
                            string resStr = JsonConvert.SerializeObject(_Queue[0]);
                            _Queue.RemoveAt(0);
                            _Writer = new BinaryWriter(_RStream);
                            _Writer.Write(resStr);
                            _Writer.Flush();
                        }
                        catch (Exception)
                        {
                            OnClientStatusChanged(ClientStatus.ClientDisconnectedError);
                            break;
                        }
                    }
                }
                OnClientStatusChanged(ClientStatus.ClientDisconnected);
            });
        }
        private void PreProcessServerMessage(string serverStr, MessageBase msgBase)
        {
            if (msgBase.Flag == 1)
                OnClientStatusChanged(ClientStatus.ClientDisconnected);
            {
                object obj = JsonConvert.DeserializeObject(serverStr, msgBase.MsgType);
                OnMessageRecieved(msgBase);
            }
        }
        private void PreProcessClientMessage(string msgStr, MessageBase msgObj)
        {
            if (msgObj.Flag == 1)
                OnClientStatusChanged(ClientStatus.ClientDisconnected);
            else
            {
                object obj = JsonConvert.DeserializeObject(msgStr, msgObj.MsgType);
                OnMessageRecieved(msgObj);
            }
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
                    _IsConnected = false;
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
        private void Dispose()
        {
            _Reader.Dispose();
            _Writer.Dispose();
            _RStream.Dispose();
            _Client.Close();
            _Client.Dispose();
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
