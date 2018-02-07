using System;
using System.Net;
using System.Net.Sockets;

namespace ITI4InARow.Module.Client
{
    public class ClientCore
    {
        TcpClient _Client;
        public void ConnectClient(byte[] ipAddress, int port)
        {
            _Client = new TcpClient();
            try
            {
                _Client.Connect("RedHawk", port);
                OnClientStatusChanged(ClientStatus.ClientConnected);
            }
            catch (SocketException)
            { OnClientStatusChanged(ClientStatus.ConnectionError); }
        }
        public void DisconnectClient()
        {
            _Client.Close();
            OnClientStatusChanged(ClientStatus.ClientDisconnected);
        }

        public event EventHandler<ClientActionEventArgs> ClientStatusChanged;
        private void OnClientStatusChanged(ClientStatus status)
        {
            ClientStatusChanged?.Invoke(this, new ClientActionEventArgs(status, null));
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
        public object Data { get; private set; }
        public ClientActionEventArgs(ClientStatus status, object data)
        {
            Status = status;
            Data = data;
        }
    }
    public enum ClientStatus
    {
        ClientConnected,
        ClientDisconnected,
        ConnectionError
    }
}
