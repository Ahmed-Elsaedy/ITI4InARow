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

        public event EventHandler<ClientActionEventArgs> ClientStatusChanged;

        private void ClientMainThread(TcpClient request)
        {
            _LastKeepALive = DateTime.Now;
            OnClientStatusChanged(ClientStatus.ListeningForServer);
            using (NetworkStream stream = request.GetStream())
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        bool flag4;
                        goto Label_00E6;
                        Label_002E:
                        this.MonitorKeepALiveTime();
                        try
                        {
                            if (stream.DataAvailable)
                            {
                                string str = reader.ReadString();
                                MessageBase msgBase = JsonConvert.DeserializeObject<MessageBase>(str);
                                this.ReadingMessageFlag(str, msgBase);
                            }
                            if (this._Queue.Count > 0)
                            {
                                MessageBase base3 = _Queue[0];
                                if (base3.Flag == MessageFlag.KeepAlive)
                                {
                                    OnClientStatusChanged(ClientStatus.SendingKeepALiveFlag);
                                }
                                else
                                {
                                    OnClientStatusChanged(ClientStatus.SendingClientMessage);
                                }
                                string str2 = JsonConvert.SerializeObject(_Queue[0]);
                                _Queue.RemoveAt(0);
                                writer.Write(str2);
                                writer.Flush();
                            }
                        }
                        catch (Exception)
                        {
                            OnClientStatusChanged(ClientStatus.ConnectionException);
                            goto Label_0115;
                        }
                        Label_00E6:
                        flag4 = true;
                        goto Label_002E;
                    }
                }
            }
            Label_0115:
            OnClientStatusChanged(ClientStatus.ClientDisconnected);
        }

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
                this.OnClientStatusChanged(ClientStatus.ConnectionError);
            }
        }

        public void DisconnectClient()
        {
            this.OnClientStatusChanged(ClientStatus.ClientDisconnected);
        }

        private void MonitorKeepALiveTime()
        {
            TimeSpan span = (TimeSpan)(DateTime.Now - this._LastKeepALive);
            if (span.TotalSeconds > 2.0)
            {
                this._LastKeepALive = DateTime.Now;
                MessageBase message = new MessageBase
                {
                    Flag = MessageFlag.KeepAlive
                };
                this.SendMessageToServer(message);
            }
        }

        private void OnClientStatusChanged(ClientStatus status)
        {
            ClientStatus status2 = status;
            if ((status2 - 2) <= ClientStatus.ClientConnected)
            {
                try
                {
                    this._Client.Close();
                }
                catch
                {
                }
            }
            if (this.ClientStatusChanged != null)
            {
                EventHandler<ClientActionEventArgs> clientStatusChanged = this.ClientStatusChanged;
                clientStatusChanged(this, new ClientActionEventArgs(status));
            }
            else
            {
                EventHandler<ClientActionEventArgs> expressionStack_2C_0 = this.ClientStatusChanged;
            }
        }

        protected virtual void OnGameUpdateMessage(GameUpdateMessage msg)
        {
        }

        protected virtual void OnRegisterMessage(ProfileUpdateMessage msg)
        {
        }

        protected virtual void OnRoomUpdateMessage(RoomUpdateMessage msg)
        {
        }

        private void ProcessServerMessage(string serverStr, MessageBase msgBase)
        {
            switch (msgBase.MsgType)
            {
                case MessageType.ProfileUpdateMessage:
                    this.OnRegisterMessage(JsonConvert.DeserializeObject<ProfileUpdateMessage>(serverStr));
                    break;

                case MessageType.RoomUpdateMessage:
                    this.OnRoomUpdateMessage(JsonConvert.DeserializeObject<RoomUpdateMessage>(serverStr));
                    break;

                case MessageType.GameUpdateMessage:
                    this.OnGameUpdateMessage(JsonConvert.DeserializeObject<GameUpdateMessage>(serverStr));
                    break;
            }
        }

        private void ReadingMessageFlag(string serverStr, MessageBase msgBase)
        {
            if (msgBase.Flag == MessageFlag.KeepAlive)
            {
                this.OnClientStatusChanged(ClientStatus.ReceivingKeepALiveFlag);
            }
            else
            {
                this.OnClientStatusChanged(ClientStatus.ProcessingIncommingMessage);
                this.ProcessServerMessage(serverStr, msgBase);
            }
        }

        public void SendMessageToServer(MessageBase message)
        {
            this._Queue.Add(message);
        }
    }
}

