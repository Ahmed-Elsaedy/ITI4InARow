namespace ITI4InARow.Module.Server
{
    using ITI4InARow.Module.Core;
    using System;
    using System.Collections.Generic;

    public class GameServer : ServerCore
    {
        private Dictionary<int, RoomUpdateMessage> _RoomsMessages;
        private Dictionary<int, ServerRoom> _RoomsData;

        public GameServer(byte[] ipAddress, int port) : base(ipAddress, port)
        {
            _RoomsMessages = new Dictionary<int, RoomUpdateMessage>();
            _RoomsData = new Dictionary<int, ServerRoom>();
        }
        protected override void OnProfileUpdateMessage(ServerClient client, ProfileUpdateMessage msg)
        {
            client.NickName = msg.Name;
            SendMessageToClient(client, msg);
        }
        protected override void OnRoomUpdateMessage(ServerClient client, RoomUpdateMessage msg)
        {
            RoomUpdateMessage message;
            switch (msg.UpdateState)
            {
                case RoomUpdateState.NewRoomRequest:
                    msg.Player1ID = client.ClientID;
                    msg.RoomID = client.ClientID + new Random().Next(100);
                    _RoomsMessages.Add(msg.RoomID, msg);
                    SendMessageToClient(client, msg);
                    message = msg.Copy();
                    message.UpdateState = RoomUpdateState.Broadcast;
                    BroadcastToClients(message, client);
                    break;
                case RoomUpdateState.NewRoomRollback:
                    _RoomsMessages.Remove(msg.RoomID);
                    msg.UpdateState = RoomUpdateState.NewRoomRollback;
                    SendMessageToClient(client, msg);
                    message = msg.Copy();
                    message.Player1ID = 0;
                    message.Player2ID = 0;
                    message.UpdateState = RoomUpdateState.Broadcast;
                    BroadcastToClients(message, client);
                    break;
                case RoomUpdateState.Player2Connected:
                    _RoomsMessages[msg.RoomID].Player2ID = client.ClientID;
                    msg.Player2ID = client.ClientID;
                    msg.UpdateState = RoomUpdateState.Player2Connected;
                    SendMessageToClient(base[msg.Player1ID], msg);
                    break;
                case RoomUpdateState.RoomComplete:
                    message = msg.Copy();
                    message.UpdateState = RoomUpdateState.Broadcast;
                    BroadcastToClients(message, client);
                    GameUpdateMessage message2 = new GameUpdateMessage { RoomID = msg.RoomID };
                    message2.UpdateStatus = GameUpdateStatus.GameStarted;
                    SendMessageToClient(base[msg.Player1ID], message2);
                    SendMessageToClient(base[msg.Player2ID], message2.Copy());
                    GameUpdateMessage message3 = new GameUpdateMessage
                    {
                        RoomID = msg.RoomID,
                        PlayerID = msg.Player1ID,
                        UpdateStatus = GameUpdateStatus.PlayerMove,
                        TokenPosition = -1
                    };
                    SendMessageToClient(base[msg.Player1ID], message3);
                    break;
            }
        }
        protected override void OnGameUpdateMessage(ServerClient client, GameUpdateMessage msg)
        {
            RoomUpdateMessage message;
            switch (msg.UpdateStatus)
            {
                //handling masgs from clint during game 
                case GameUpdateStatus.PlayerMove:
                    message = _RoomsMessages[msg.RoomID];
                    msg.PlayerID = (msg.PlayerID == message.Player1ID) ? message.Player2ID : message.Player1ID;
                    SendMessageToClient(base[msg.PlayerID], msg);
                    _RoomsData[msg.RoomID].gameBourdlogic[msg.TokenPosition - 1] = msg.PlayerID; //here i got te move saved in server with the id of its pleyaer
                    bool win = GameAction(client, msg);
                    _RoomsData[msg.RoomID]._RoomMoveCounter[client.ClientID] += 1;
                    if (_RoomsData[msg.RoomID]._RoomMoveCounter[msg.RoomID] == 42 && win == false)
                    {
                        GameUpdateMessage drawRespMsg = msg.Copy();
                        drawRespMsg.UpdateStatus = GameUpdateStatus.GameDraw;
                        msg.IsGameRunning = false;
                        //now send draw msg  to both players 
                        SendMessageToClient(this[_RoomsMessages[msg.RoomID].Player1ID], drawRespMsg);
                        SendMessageToClient(this[_RoomsMessages[msg.RoomID].Player1ID], drawRespMsg);
                        //Game Draw
                    }
                    else if (win)
                    {
                        GameUpdateMessage msgWin = msg.Copy();
                        msgWin.UpdateStatus = GameUpdateStatus.win;
                        SendMessageToClient(this[msgWin.PlayerID], msg);
                        //sent win msg
                        GameUpdateMessage msgLose = msg.Copy();
                        msgLose.UpdateStatus = GameUpdateStatus.lose;
                        SendMessageToClient(this[(msg.PlayerID == message.Player1ID) ? message.Player2ID : message.Player1ID], msgLose);
                        //send lose msg
                    }
                    else if (!win)
                    {
                        GameUpdateMessage msgOtherPlayerPlay = msg.Copy();
                        msgOtherPlayerPlay.UpdateStatus = GameUpdateStatus.PlayerMove;
                        SendMessageToClient(this[(msg.PlayerID == message.Player1ID) ? message.Player2ID : message.Player1ID], msgOtherPlayerPlay);
                    }


                    break;
                case GameUpdateStatus.GameLeave:
                    {
                        message = _RoomsMessages[msg.RoomID];
                        SendMessageToClient(base[message.Player1ID], msg);
                        SendMessageToClient(base[message.Player2ID], msg.Copy());
                        _RoomsMessages.Remove(msg.RoomID);
                        _RoomsData.Remove(msg.RoomID);
                        RoomUpdateMessage message2 = new RoomUpdateMessage { RoomID = msg.RoomID };
                        message2.UpdateState = RoomUpdateState.Broadcast;
                        message2.Player1ID = 0;
                        message2.Player2ID = 0;
                        BroadcastToClients(message2, null);
                        break;
                    }
            }
        }


        bool GameAction(ServerClient client, GameUpdateMessage msg)
        {
            int x = 1;
            if (Helper.NorthBanned.IndexOf(msg.TokenPosition) == -1)
            {
                if (GamePlan(msg, ref x, CheckPosition.NORTH))
                {
                    //isGameRunning = false;
                    //MessageBox.Show(ovalClicked.FillColor.ToString() + " is win North");
                }
            }
            if (Helper.SouthBanned.IndexOf(msg.TokenPosition) == -1)
            {
                if (GamePlan(msg, ref x, CheckPosition.SOUTH))
                {
                    msg.IsGameRunning = false;
                    //MessageBox.Show(ovalClicked.FillColor.ToString() + " is win south");
                    return true;
                }
            }
            /////////////////////////////////////////////
            x = 1;
            if (GamePlan(msg, ref x, CheckPosition.WEST))
            {
                //isGameRunning = false;
                //MessageBox.Show(ovalClicked.FillColor.ToString() + " is win west");
            }
            if (GamePlan(msg, ref x, CheckPosition.EAST))
            {
                msg.IsGameRunning = false;
                //MessageBox.Show(ovalClicked.FillColor.ToString() + " is win east");
                return true;
            }
            //////////////////////////////////////////////
            x = 1;
            if (Helper.NorthBanned.IndexOf(msg.TokenPosition) == -1)
            {
                if (GamePlan(msg, ref x, CheckPosition.NORTH_EAST))
                {
                    //isGameRunning = false;
                    //MessageBox.Show(ovalClicked.FillColor.ToString() + " is win north west");
                }
            }
            if (Helper.SouthBanned.IndexOf(msg.TokenPosition) == -1)
            {
                if (GamePlan(msg, ref x, CheckPosition.SOUTH_WEST))
                {
                    msg.IsGameRunning = false;
                    // MessageBox.Show(ovalClicked.FillColor.ToString() + " is win south west");
                    return true;
                }
            }
            ////////////////////////////////////////////////
            x = 1;

            if (GamePlan(msg, ref x, CheckPosition.NORTH_WEST))
            {
                //isGameRunning = false;
                //MessageBox.Show(ovalClicked.FillColor.ToString() + " is win north west");
            }

            if (GamePlan(msg, ref x, CheckPosition.SOUTH_EAST))
            {
                msg.IsGameRunning = false;
                //MessageBox.Show(ovalClicked.FillColor.ToString() + " is win south east");
                return true;
            }
            /////////////////
            return false;
        }
        bool GamePlan(GameUpdateMessage msg, ref int x, CheckPosition cp)
        {
            if (x < 4)
            {
                int TokenIndex = msg.TokenPosition - 1;
                int leftTokenIndex = TokenIndex + (int)cp;
                if (leftTokenIndex >= 0 && leftTokenIndex < 42)
                {
                    //if (ovalClicked.FillColor.Equals(((OvalShape)shapeContainer1.Shapes.get_Item(leftTokenIndex)).FillColor))
                    if (_RoomsData[msg.RoomID].gameBourdlogic[msg.TokenPosition] == _RoomsData[msg.RoomID].gameBourdlogic[leftTokenIndex])
                    {
                        x++;
                        GameUpdateMessage nextToken = msg.Copy();
                        nextToken.TokenPosition = leftTokenIndex;
                        return GamePlan(nextToken, ref x, cp);
                    }
                    else
                    {
                        return false;
                    }
                }
                else return false;
            }
            return true;
        }
    }

    public class ServerRoom
    {
        public int RoomID { get; set; }
        public Dictionary<int, int> _RoomMoveCounter;
        public int[] gameBourdlogic = new int[42];

        public ServerRoom()
        {
            _RoomMoveCounter = new Dictionary<int, int>();
        }
    }
}

