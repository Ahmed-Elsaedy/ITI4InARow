namespace ITI4InARow.Module.Server
{
    using ITI4InARow.Module.Core;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;

    public class ServerCore
    {
        private List<ServerClient> _Clients;
        private TcpListener _Server;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler<MessageRevievedEventArgs> MessageRecieved;

        [field: CompilerGenerated, DebuggerBrowsable(0)]
        public event EventHandler<ServerActionEventArgs> ServerStatusChanged;

        public ServerCore(byte[] ipAddress, int port)
        {
            this._Server = new TcpListener(new IPAddress(ipAddress), port);
            this._Clients = new List<ServerClient>();
        }

        public void BroadcastToClients(MessageBase msg, ServerClient source = null)
        {
            if (source != null)
            {
                (from x in this._Clients
                    where x.ClientID != source.ClientID
                    select x).ToList<ServerClient>().ForEach(x => x.Queue.Add(msg));
            }
            else
            {
                foreach (ServerClient client in this._Clients)
                {
                    client.Queue.Add(msg);
                }
            }
        }

        private void MonitorKeepALiveTime(ServerClient client)
        {
            TimeSpan span = (TimeSpan) (DateTime.Now - client.LastKeepALive);
            if (span.TotalSeconds > 2.0)
            {
                client.LastKeepALive = DateTime.Now;
                MessageBase msg = new MessageBase {
                    Flag = MessageFlag.KeepAlive
                };
                this.SendMessageToClient(client, msg);
            }
        }

        protected virtual void OnGameUpdateMessage(ServerClient client, GameUpdateMessage msg)
        {
        }

        private void OnMessageRecieved(ServerClient client, string msgStr, MessageBase msgObj)
        {
            if (this.MessageRecieved != null)
            {
                EventHandler<MessageRevievedEventArgs> messageRecieved = this.MessageRecieved;
                messageRecieved(this, new MessageRevievedEventArgs(msgObj, client.Client.Client.Handle.ToInt32()));
            }
            else
            {
                EventHandler<MessageRevievedEventArgs> expressionStack_A_0 = this.MessageRecieved;
            }
            this.OnServerStatusChanged(ServerStatus.ProcessingIncommingMessage, client);
            this.ProcessClientMessage(client, msgStr, msgObj);
        }

        protected virtual void OnRegisterMessage(ServerClient client, ProfileUpdateMessage msg)
        {
        }

        protected virtual void OnRoomUpdateMessage(ServerClient client, RoomUpdateMessage msg)
        {
        }

        private void OnServerStatusChanged(ServerStatus action, ServerClient client)
        {
            switch (action)
            {
                case ServerStatus.ServerStopped:
                    foreach (ServerClient client2 in this._Clients)
                    {
                        client.Client.Close();
                    }
                    this._Server.Stop();
                    break;

                case ServerStatus.ConnectionException:
                case ServerStatus.ClientDisconnected:
                    try
                    {
                        client.Client.Close();
                    }
                    catch
                    {
                    }
                    this._Clients.Remove(client);
                    break;

                case ServerStatus.ClientConnected:
                    this._Clients.Add(client);
                    break;
            }
            if (this.ServerStatusChanged != null)
            {
                EventHandler<ServerActionEventArgs> serverStatusChanged = this.ServerStatusChanged;
                serverStatusChanged(this, new ServerActionEventArgs(action, client));
            }
            else
            {
                EventHandler<ServerActionEventArgs> expressionStack_AB_0 = this.ServerStatusChanged;
            }
        }

        protected void ProcessClientMessage(ServerClient client, string msgStr, MessageBase msgObj)
        {
            switch (msgObj.MsgType)
            {
                case MessageType.ProfileUpdateMessage:
                    this.OnRegisterMessage(client, JsonConvert.DeserializeObject<ProfileUpdateMessage>(msgStr));
                    break;

                case MessageType.RoomUpdateMessage:
                    this.OnRoomUpdateMessage(client, JsonConvert.DeserializeObject<RoomUpdateMessage>(msgStr));
                    break;

                case MessageType.GameUpdateMessage:
                    this.OnGameUpdateMessage(client, JsonConvert.DeserializeObject<GameUpdateMessage>(msgStr));
                    break;
            }
        }

        private void ReadingMessageFlag(ServerClient client, string msgStr, MessageBase msgObj)
        {
            if (msgObj.Flag == MessageFlag.KeepAlive)
            {
                this.OnServerStatusChanged(ServerStatus.ReceivingKeepALiveFlag, client);
            }
            else
            {
                this.OnMessageRecieved(client, msgStr, msgObj);
            }
        }

        public void SendMessageToClient(ServerClient client, MessageBase msg)
        {
            client.Queue.Add(msg);
        }

        private void ServerClientThread(ServerClient serverClient)
        {
            this.OnServerStatusChanged(ServerStatus.ListeningForClient, serverClient);
            using (NetworkStream stream = serverClient.Client.GetStream())
            {
                using (BinaryReader reader = new BinaryReader(stream))
                {
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        bool flag4;
                        goto Label_00D6;
                    Label_0029:;
                        try
                        {
                            if (stream.DataAvailable)
                            {
                                string str = reader.ReadString();
                                MessageBase msgObj = JsonConvert.DeserializeObject<MessageBase>(str);
                                this.ReadingMessageFlag(serverClient, str, msgObj);
                            }
                            if (serverClient.Queue.Count > 0)
                            {
                                MessageBase base3 = serverClient.Queue[0];
                                if (base3.Flag == MessageFlag.KeepAlive)
                                {
                                    this.OnServerStatusChanged(ServerStatus.SendingKeepALiveFlag, serverClient);
                                }
                                else
                                {
                                    this.OnServerStatusChanged(ServerStatus.SendingServerMessage, serverClient);
                                }
                                string str2 = JsonConvert.SerializeObject(base3);
                                serverClient.Queue.RemoveAt(0);
                                writer.Write(str2);
                                writer.Flush();
                            }
                        }
                        catch (Exception)
                        {
                            this.OnServerStatusChanged(ServerStatus.ConnectionException, serverClient);
                            goto Label_0105;
                        }
                    Label_00D6:
                        flag4 = true;
                        goto Label_0029;
                    }
                }
            }
        Label_0105:
            this.OnServerStatusChanged(ServerStatus.ClientDisconnected, serverClient);
        }

        private void ServerMainThread()
        {
            this.OnServerStatusChanged(ServerStatus.StartWaitingForClients, null);
            while (true)
            {
                foreach (ServerClient client in this._Clients)
                {
                    MessageBase msg = new MessageBase {
                        Flag = MessageFlag.KeepAlive
                    };
                    this.SendMessageToClient(client, msg);
                }
                if (this._Server.Pending())
                {
                    this.OnServerStatusChanged(ServerStatus.IncommingClient, null);
                    TcpClient client2 = this._Server.AcceptTcpClient();
                    ServerClient serverClient = new ServerClient(client2);
                    this.OnServerStatusChanged(ServerStatus.ClientConnected, serverClient);
                    Task.Run((Action) (() => this.ServerClientThread(serverClient)));
                }
                Thread.Sleep(0x7d0);
            }
        }

        public void StartServer()
        {
            this._Server.Start();
            this.OnServerStatusChanged(ServerStatus.ServerStarted, null);
            Task.Run(new Action(this.ServerMainThread));
        }

        public void StopServer()
        {
            this.OnServerStatusChanged(ServerStatus.ServerStopped, null);
        }

        protected ServerClient this[int handle] =>
            this._Clients.SingleOrDefault<ServerClient>(x => (x.ClientID == handle));
    }
}

