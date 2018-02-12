namespace ITI4InARow.Module.Server
{
    using ITI4InARow.Module.Core;
    using System;
    using System.Collections.Generic;

    public class GameServer : ServerCore
    {
        private Dictionary<int, RoomUpdateMessage> _RoomsData;
        private Dictionary<int, int[,]> _RoomsMatrix;

        public GameServer(byte[] ipAddress, int port) : base(ipAddress, port)
        {
            this._RoomsData = new Dictionary<int, RoomUpdateMessage>();
            this._RoomsMatrix = new Dictionary<int, int[,]>();
        }

        protected override void OnGameUpdateMessage(ServerClient client, GameUpdateMessage msg)
        {
            RoomUpdateMessage message;
            switch (msg.UpdateStatus)
            {
                case GameUpdateStatus.PlayerMove:
                    message = this._RoomsData[msg.RoomID];
                    msg.PlayerID = (msg.PlayerID == message.Player1ID) ? message.Player2ID : message.Player1ID;
                    base.SendMessageToClient(base[msg.PlayerID], msg);
                    break;

                case GameUpdateStatus.GameLeave:
                {
                    message = this._RoomsData[msg.RoomID];
                    base.SendMessageToClient(base[message.Player1ID], msg);
                    base.SendMessageToClient(base[message.Player2ID], msg.Copy());
                    this._RoomsData.Remove(msg.RoomID);
                    this._RoomsMatrix.Remove(msg.RoomID);
                    RoomUpdateMessage message2 = new RoomUpdateMessage {
                        RoomID = msg.RoomID
                    };
                    message2.UpdateState = RoomUpdateState.Broadcast;
                    message2.Player1ID = 0;
                    message2.Player2ID = 0;
                    base.BroadcastToClients(message2, null);
                    break;
                }
            }
        }

        protected override void OnRegisterMessage(ServerClient client, ProfileUpdateMessage msg)
        {
            client.NickName = msg.Name;
            base.SendMessageToClient(client, msg);
        }

        protected override void OnRoomUpdateMessage(ServerClient client, RoomUpdateMessage msg)
        {
            RoomUpdateMessage message;
            switch (msg.UpdateState)
            {
                case RoomUpdateState.NewRoomRequest:
                    msg.Player1ID = client.ClientID;
                    msg.RoomID = client.ClientID + new Random().Next(100);
                    this._RoomsData.Add(msg.RoomID, msg);
                    base.SendMessageToClient(client, msg);
                    message = msg.Copy();
                    message.UpdateState = RoomUpdateState.Broadcast;
                    base.BroadcastToClients(message, client);
                    break;

                case RoomUpdateState.NewRoomRollback:
                    this._RoomsData.Remove(msg.RoomID);
                    msg.UpdateState = RoomUpdateState.NewRoomRollback;
                    base.SendMessageToClient(client, msg);
                    message = msg.Copy();
                    message.Player1ID = 0;
                    message.Player2ID = 0;
                    message.UpdateState = RoomUpdateState.Broadcast;
                    base.BroadcastToClients(message, client);
                    break;

                case RoomUpdateState.Player2Connected:
                    this._RoomsData[msg.RoomID].Player2ID = client.ClientID;
                    msg.Player2ID = client.ClientID;
                    msg.UpdateState = RoomUpdateState.Player2Connected;
                    base.SendMessageToClient(base[msg.Player1ID], msg);
                    break;

                case RoomUpdateState.RoomComplete:
                {
                    this._RoomsMatrix.Add(msg.RoomID, new int[7, 7]);
                    message = msg.Copy();
                    message.UpdateState = RoomUpdateState.Broadcast;
                    base.BroadcastToClients(message, client);
                    GameUpdateMessage message2 = new GameUpdateMessage {
                        RoomID = msg.RoomID
                    };
                    message2.UpdateStatus = GameUpdateStatus.GameStarted;
                    base.SendMessageToClient(base[msg.Player1ID], message2);
                    base.SendMessageToClient(base[msg.Player2ID], message2.Copy());
                    GameUpdateMessage message3 = new GameUpdateMessage {
                        RoomID = msg.RoomID,
                        PlayerID = msg.Player1ID,
                        UpdateStatus = GameUpdateStatus.PlayerMove
                    };
                    base.SendMessageToClient(base[msg.Player1ID], message3);
                    break;
                }
            }
        }
    }
}

