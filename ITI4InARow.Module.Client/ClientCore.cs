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
        private DateTime _LastKeepALive;
        private List<MessageBase> _Queue = null;
        public void ConnectClient(byte[] ipAddress, int port)
        {
            _Client = new TcpClient();
            _Queue = new List<MessageBase>();
            try
            {
                _Client.Connect(new IPAddress(ipAddress), port);
                OnClientStatusChanged(ClientStatus.ClientConnected);
                Task.Run(() => ClientMainThread(_Client));
            }
            catch (SocketException)
            { OnClientStatusChanged(ClientStatus.ConnectionError); }
        }
        public void SendMessageToServer(MessageBase message)
        {
            _Queue.Add(message);
        }
        private void ClientMainThread(TcpClient request)
        {
            _LastKeepALive = DateTime.Now;
            OnClientStatusChanged(ClientStatus.ListeningForServer);
            using (NetworkStream _RStream = request.GetStream())
            using (BinaryReader _Reader = new BinaryReader(_RStream))
            using (BinaryWriter _Writer = new BinaryWriter(_RStream))
                while (true)
                {
                    MonitorKeepALiveTime();
                    try
                    {
                        if (_RStream.DataAvailable)
                        {
                            var serverStr = _Reader.ReadString();
                            MessageBase msgBase = JsonConvert.DeserializeObject<MessageBase>(serverStr);
                            ReadingMessageFlag(serverStr, msgBase);
                        }
                        if (_Queue.Count > 0)
                        {
                            var msg = _Queue[0];
                            if (msg.Flag == MessageFlag.KeepAlive)
                                OnClientStatusChanged(ClientStatus.SendingKeepALiveFlag);
                            else
                                OnClientStatusChanged(ClientStatus.SendingClientMessage);
                            string resStr = JsonConvert.SerializeObject(_Queue[0]);
                            _Queue.RemoveAt(0);
                            _Writer.Write(resStr);
                            _Writer.Flush();
                        }
                    }
                    catch (Exception ex)
                    {
                        OnClientStatusChanged(ClientStatus.ConnectionException);
                        break;
                    }
                }
            OnClientStatusChanged(ClientStatus.ClientDisconnected);
        }
        private void ReadingMessageFlag(string serverStr, MessageBase msgBase)
        {
            if (msgBase.Flag == MessageFlag.KeepAlive)
                OnClientStatusChanged(ClientStatus.ReceivingKeepALiveFlag);
            else
            {
                object obj = JsonConvert.DeserializeObject(serverStr, msgBase.MsgType);
                OnMessageRecieved(msgBase);
            }
        }
        private void OnMessageRecieved(MessageBase msgBase)
        {
            OnClientStatusChanged(ClientStatus.ProcessingIncommingMessage);
            ProcessServerMessage(msgBase);
            MessageRecieved?.Invoke(this, new MessageRevievedEventArgs(msgBase, msgBase.ClientID));
        }
        protected virtual void ProcessServerMessage(MessageBase msgBase)
        {

        }
        private void MonitorKeepALiveTime()
        {
            var period = DateTime.Now - _LastKeepALive;
            if (period.TotalSeconds > 2)
            {
                _LastKeepALive = DateTime.Now;
                MessageBase keepAliveMsg = new MessageBase() { Flag = MessageFlag.KeepAlive };
                SendMessageToServer(keepAliveMsg);
            }
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
                case ClientStatus.ConnectionException:
                    try { _Client.Close(); }
                    catch { }
                    break;
            }
            ClientStatusChanged?.Invoke(this, new ClientActionEventArgs(status));
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
        ConnectionError,
        ClientConnected,
        ClientDisconnected,
        ConnectionException,
        ListeningForServer,
        ReadingServerStream,
        ProcessingIncommingMessage,
        SendingClientMessage,
        SendingKeepALiveFlag,
        ReceivingKeepALiveFlag
    }
}
