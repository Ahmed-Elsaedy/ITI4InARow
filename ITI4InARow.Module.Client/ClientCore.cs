namespace ITI4InARow.Module.Client
{
    using ITI4InARow.Module.Core;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

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
            {
                OnClientStatusChanged(ClientStatus.ConnectionError);
            }
        }
        private void ClientMainThread(TcpClient request)
        {
            _LastKeepALive = DateTime.Now;
            OnClientStatusChanged(ClientStatus.ListeningForServer);
            using (NetworkStream stream = request.GetStream())
            using (BinaryReader reader = new BinaryReader(stream))
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                while (true)
                {
                    MonitorKeepALiveTime();
                    try
                    {
                        if (stream.DataAvailable)
                        {
                            string str = reader.ReadString();
                            MessageBase msgBase = JsonConvert.DeserializeObject<MessageBase>(str);
                            ReadingMessageFlag(str, msgBase);
                        }
                        if (_Queue.Count > 0)
                        {
                            MessageBase base3 = _Queue[0];
                            //if (base3.Flag == MessageFlag.KeepAlive)
                            //    OnClientStatusChanged(ClientStatus.SendingKeepALiveFlag);
                            //else
                                OnClientStatusChanged(ClientStatus.SendingClientMessage);
                            string str2 = JsonConvert.SerializeObject(_Queue[0]);
                            _Queue.RemoveAt(0);
                            writer.Write(str2);
                            writer.Flush();
                        }
                    }
                    catch (Exception)
                    {
                        OnClientStatusChanged(ClientStatus.ConnectionException);
                        break;
                    }
                }
            }
            OnClientStatusChanged(ClientStatus.ClientDisconnected);
        }
        public void DisconnectClient()
        {
            OnClientStatusChanged(ClientStatus.ClientDisconnected);
        }
        public void SendMessageToServer(MessageBase message)
        {
            _Queue.Add(message);
        }
        private void MonitorKeepALiveTime()
        {
            TimeSpan span = (TimeSpan)(DateTime.Now - _LastKeepALive);
            if (span.TotalSeconds > 2.0)
            {
                _LastKeepALive = DateTime.Now;
                MessageBase message = new MessageBase
                {
                    Flag = MessageFlag.KeepAlive
                };
                SendMessageToServer(message);
            }
        }
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
        private void ProcessServerMessage(string serverStr, MessageBase msgBase)
        {
            switch (msgBase.MsgType)
            {
                case MessageType.ProfileUpdateMessage:
                    OnProfileUpdateMessage(JsonConvert.DeserializeObject<ProfileUpdateMessage>(serverStr));
                    break;

                case MessageType.RoomUpdateMessage:
                    OnRoomUpdateMessage(JsonConvert.DeserializeObject<RoomUpdateMessage>(serverStr));
                    break;

                case MessageType.GameUpdateMessage:
                    OnGameUpdateMessage(JsonConvert.DeserializeObject<GameUpdateMessage>(serverStr));
                    break;
            }
        }
        private void ReadingMessageFlag(string serverStr, MessageBase msgBase)
        {
            //if (msgBase.Flag == MessageFlag.KeepAlive)
            //    OnClientStatusChanged(ClientStatus.ReceivingKeepALiveFlag);
            //else
            {
                OnClientStatusChanged(ClientStatus.ProcessingIncommingMessage);
                ProcessServerMessage(serverStr, msgBase);
            }
        }
        protected virtual void OnProfileUpdateMessage(ProfileUpdateMessage msg)
        {
        }
        protected virtual void OnRoomUpdateMessage(RoomUpdateMessage msg)
        {
        }
        protected virtual void OnGameUpdateMessage(GameUpdateMessage msg)
        {
        }

        public event EventHandler<ClientActionEventArgs> ClientStatusChanged;
    }
}

