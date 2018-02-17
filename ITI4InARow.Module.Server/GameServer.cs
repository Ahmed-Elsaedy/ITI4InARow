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
            client.NickName = msg.PlayerName;
            client.PreferedColor = msg.PlayerColor;
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
                    _RoomsData.Add(msg.RoomID, (new ServerRoom()
                    {
                    RoomID = msg.RoomID, _RoomMoveCounter = 0 ,Player1Color=client.PreferedColor
                    }));///////elcolor hna
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
                    _RoomsData[msg.RoomID].Player2Color = client.PreferedColor;//////////////
                    SendMessageToClient(base[msg.Player1ID], msg);
                    break;
                case RoomUpdateState.RoomComplete:
                    message = msg.Copy();
                    message.UpdateState = RoomUpdateState.Broadcast;
                    BroadcastToClients(message, client);
                    GameUpdateMessage messageForPlayer1 = new GameUpdateMessage {
                        RoomID = msg.RoomID,
                        UpdateStatus = GameUpdateStatus.GameStarted,
                        Player2Color = _RoomsData[msg.RoomID].Player2Color,///loon el player el tany b2a agebh mneeen ?! 
                        PlayerID = msg.Player1ID,
                        IsGameRunning = true
                      };
                   
                    SendMessageToClient(base[msg.Player1ID], messageForPlayer1);
                    //messageForPlayer1.IsGameRunning = false;
                    ///mafrood hna n3ks el alwan 3shan kl la3b y3rf loon el tany =D msh bnl3b bnfs loon ely create el room 
                    GameUpdateMessage messageForPlayer2 = new GameUpdateMessage
                    {
                        RoomID = msg.RoomID,
                        UpdateStatus = GameUpdateStatus.GameStarted,
                        Player2Color = _RoomsData[msg.RoomID].Player1Color, //////
                        PlayerID = msg.Player2ID,
                        IsGameRunning = false // disable player 2
                    };
                    SendMessageToClient(base[msg.Player2ID], messageForPlayer2);
                    break;
                case RoomUpdateState.newSpectatorReq:
                    _RoomsData[msg.RoomID].spectators.Add(client);
                    string [] viewspac = new string[42];
                    for (int i = 0; i < viewspac.Length; i++)
                    {
                        if (_RoomsData[msg.RoomID].gameBoardlogic[i] == msg.Player1ID)
                        {
                            viewspac[i] = _RoomsData[msg.RoomID].Player1Color;
                        }
                        else if (_RoomsData[msg.RoomID].gameBoardlogic[i] == msg.Player2ID)
                        {
                            viewspac[i] = _RoomsData[msg.RoomID].Player2Color;
                        }
                        else
                        {
                            viewspac[i] = "White";
                        }
                    }
                    GameUpdateMessage msgForSpectator = new GameUpdateMessage()
                    {
                        UpdateStatus = GameUpdateStatus.SpectatorJoin,
                        viewSpectatorBoard = viewspac
                    };
                    
                    SendMessageToClient(client,msgForSpectator);
                    break;
            }
        }
        protected override void OnGameUpdateMessage(ServerClient client, GameUpdateMessage msg)
        {
            RoomUpdateMessage message;
            switch (msg.UpdateStatus)
            {
                //handling masgs from client during game 
                case GameUpdateStatus.PlayerMove:
                    message = _RoomsMessages[msg.RoomID];
                    _RoomsData[msg.RoomID].gameBoardlogic[msg.TokenPosition - 1] = msg.PlayerID; //here i got te move saved in server with the id of its player
                    string[] viewspac = new string[42];
                    for (int i = 0; i < viewspac.Length; i++)
                    {
                        if (_RoomsData[msg.RoomID].gameBoardlogic[i] == message.Player1ID)
                        {
                            viewspac[i] = _RoomsData[message.RoomID].Player1Color;
                        }
                        else if (_RoomsData[msg.RoomID].gameBoardlogic[i] == message.Player2ID)
                        {
                            viewspac[i] = _RoomsData[message.RoomID].Player2Color;
                        }
                        else
                        {
                            viewspac[i] = "White";
                        }
                    }
                    msg.PlayerID = (msg.PlayerID == message.Player1ID) ? message.Player2ID : message.Player1ID;
                    SendMessageToClient(base[msg.PlayerID], msg);
                    GameUpdateMessage msgtospectator = msg.Copy();
                    msgtospectator.UpdateStatus = GameUpdateStatus.viewMoveToSpectator;
                    msgtospectator.viewSpectatorBoard = viewspac;
                    foreach (ServerClient spectator in _RoomsData[msg.RoomID].spectators)
                    {
                        SendMessageToClient(spectator, msg);
                    }
                    bool win = GameAction(msg);
                    _RoomsData[msg.RoomID]._RoomMoveCounter += 1;
                    if (_RoomsData[msg.RoomID]._RoomMoveCounter == 42 && win == false)
                    {
                        GameUpdateMessage drawRespMsg = msg.Copy();
                        //drawRespMsg.IsGameRunning = false;
                        drawRespMsg.UpdateStatus = GameUpdateStatus.GameDraw;
                        //msg.IsGameRunning = false;
                        //now send draw msg  to both players 
                        SendMessageToClient(this[_RoomsMessages[msg.RoomID].Player1ID], drawRespMsg);
                        SendMessageToClient(this[_RoomsMessages[msg.RoomID].Player2ID], drawRespMsg);
                        //Game Draw
                    }
                    else if (win)
                    {
                        GameUpdateMessage msgWin = msg.Copy();
                        //msg.IsGameRunning = false;
                        msgWin.UpdateStatus = GameUpdateStatus.win;
                        SendMessageToClient(this[(msg.PlayerID == message.Player1ID) ? message.Player2ID : message.Player1ID], msgWin);
                        //sent win msg
                        GameUpdateMessage msgLose = msg.Copy();
                        msgLose.UpdateStatus = GameUpdateStatus.lose;
                        SendMessageToClient(this[msg.PlayerID], msgLose);
                        //send lose msg
                    }
                    else if (!win)
                    {
                        GameUpdateMessage msgOtherPlayerPlay = msg.Copy();
                        msgOtherPlayerPlay.UpdateStatus = GameUpdateStatus.MakeYourMove;
                        SendMessageToClient(this[msg.PlayerID], msgOtherPlayerPlay);
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

        public bool GameAction(GameUpdateMessage msg)
        {
            msg.TokenPosition--;
            int x = 1;
            if (Helper.NorthBanned.IndexOf(msg.TokenPosition) == -1)
            {
                if (GamePlan(msg, ref x, CheckPosition.NORTH))
                {
                    msg.IsGameRunning = false;
                    //MessageBox.Show(ovalClicked.FillColor.ToString() + " is win North");
                   // if (x == 4) { return true; }
                }
            }
            if (Helper.SouthBanned.IndexOf(msg.TokenPosition) == -1)
            {
                if (GamePlan(msg, ref x, CheckPosition.SOUTH))
                {
                    msg.IsGameRunning = false;
                    //MessageBox.Show(ovalClicked.FillColor.ToString() + " is win south");
                    //  if (x == 4) { return true; } /// mdam d5fl el if condition how 5las fazzz msh m7taga check
                    return true;
                }
            }
            /////////////////////////////////////////////
            x = 1;
            if (GamePlan(msg, ref x, CheckPosition.WEST))
            {
                msg.IsGameRunning = false;
               // if (x == 4) { return true; }
            }
            if (GamePlan(msg, ref x, CheckPosition.EAST))
            {
                msg.IsGameRunning = false;
                //if (x == 4) { return true; }               /// mdam d5fl el if condition how 5las fazzz msh m7taga check
                return true;
            }
            //////////////////////////////////////////////
            x = 1;
            if (Helper.NorthBanned.IndexOf(msg.TokenPosition) == -1)
            {
                if (GamePlan(msg, ref x, CheckPosition.NORTH_EAST))
                {
                    msg.IsGameRunning = false;
                   // if (x == 4) { return true; }
                }
            }
            if (Helper.SouthBanned.IndexOf(msg.TokenPosition) == -1)
            {
                if (GamePlan(msg, ref x, CheckPosition.SOUTH_WEST))
                {
                    msg.IsGameRunning = false;
                    //if (x == 4) { return true; }      /// mdam d5fl el if condition how 5las fazzz msh m7taga check
                    return true;
                }
            }
            ////////////////////////////////////////////////
            x = 1;

            if (GamePlan(msg, ref x, CheckPosition.NORTH_WEST))
            {
                msg.IsGameRunning = false;
                //MessageBox.Show(ovalClicked.FillColor.ToString() + " is win north west");
                //if (x == 4) { return true; }
            }

            if (GamePlan(msg, ref x, CheckPosition.SOUTH_EAST))
            {
                msg.IsGameRunning = false;
                //MessageBox.Show(ovalClicked.FillColor.ToString() + " is win south east");
                // if (x == 4) { return true; }     /// mdam d5fl el if condition how 5las fazzz msh m7taga check
                return true;
            }
            /////////////////
            return false;
        }
        bool GamePlan(GameUpdateMessage msg, ref int x, CheckPosition cp)
        {
            if (x < 4)
            {
                int TokenIndex = msg.TokenPosition ;
                int nextTokenIndex = TokenIndex + (int)cp;
                if (nextTokenIndex >= 0 && nextTokenIndex < 42)
                {
                    if (_RoomsData[msg.RoomID].gameBoardlogic[TokenIndex] == _RoomsData[msg.RoomID].gameBoardlogic[nextTokenIndex])
                    {
                        x++;
                        GameUpdateMessage nextToken = msg.Copy();
                        nextToken.TokenPosition = nextTokenIndex;
                        return GamePlan(nextToken, ref x, cp);
                    }
                    else { return false; }
                }
                else return false;
            }
            return true;
        }
    }

    public class ServerRoom
    {
        public int RoomID { get; set; }
        public int _RoomMoveCounter;
        public int[] gameBoardlogic;
        public List<ServerClient> spectators;
        public string Player1Color { get; set; }
        /// <summary>
        ///m7tagen nst3mlhm 
        /// </summary>
        public string Player2Color { get; set; }
        /// <summary>
        ///m7tagen nst3mlhm 
        /// </summary>
        public ServerRoom()
        {
            _RoomMoveCounter = 0;
            gameBoardlogic = new int[42];
            for (int i = 0; i < 42; i++)
            {
                gameBoardlogic[i] = -1;
            }
            spectators = new List<ServerClient>();
        }
    }
}

