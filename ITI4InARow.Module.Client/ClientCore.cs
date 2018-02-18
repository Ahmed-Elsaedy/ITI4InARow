namespace ITI4InARow.Module.Client
{
    using ITI4InARow.Module.Core;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class ClientCore
    {
        private TcpClient _Client;
        private NetworkStream _Stream;
        private BinaryReader _Reader;
        private BinaryWriter _Writer;

        public void ConnectClient(byte[] ipAddress, int port)
        {
            _Client = new TcpClient();
            try
            {
                _Client.Connect(new IPAddress(ipAddress), port);
                OnClientStatusChanged(ClientStatus.ClientConnected);
                _Stream = _Client.GetStream();
                _Writer = new BinaryWriter(_Stream);
                _Reader = new BinaryReader(_Stream);
                Task.Run(() => ClientMainThread(_Client));
            }
            catch (SocketException)
            {
                OnClientStatusChanged(ClientStatus.ConnectionError);
            }
        }
        private void ClientMainThread(TcpClient request)
        {
            OnClientStatusChanged(ClientStatus.ListeningForServer);
            while (true)
            {
                try
                {
                    string str = _Reader.ReadString();
                    MessageBase msgBase = JsonConvert.DeserializeObject<MessageBase>(str);
                    OnClientStatusChanged(ClientStatus.ProcessingIncommingMessage);
                    ProcessServerMessage(str, msgBase);
                }
                catch (Exception ex)
                {
                    OnClientStatusChanged(ClientStatus.ConnectionException);
                    OnClientStatusChanged(ClientStatus.ClientDisconnected);
                    //throw ex;
                }
            }
        }
        public void DisconnectClient()
        {
            OnClientStatusChanged(ClientStatus.ClientDisconnected);
        }
        public void SendMessageToServer(MessageBase message)
        {
            try
            {
                OnClientStatusChanged(ClientStatus.SendingClientMessage);
                string str = JsonConvert.SerializeObject(message);
                _Writer.Write(str);
                _Writer.Flush();
            }
            catch (Exception ex)
            {
                OnClientStatusChanged(ClientStatus.ConnectionException);
                OnClientStatusChanged(ClientStatus.ClientDisconnected);
                throw ex;
            }
        }
        private void OnClientStatusChanged(ClientStatus status)
        {
            switch (status)
            {
                case ClientStatus.ClientDisconnected:
                        _Reader.Dispose();
                        _Writer.Dispose();
                        _Stream.Dispose();
                        _Client.Dispose();
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

