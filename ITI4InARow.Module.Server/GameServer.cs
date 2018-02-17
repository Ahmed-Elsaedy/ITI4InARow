using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITI4InARow.Module.Core;

namespace ITI4InARow.Module.Server
{
    public class GameServer : ServerCore
    {
        List<ServerRoom> _ServerRooms;
        public GameServer(byte[] ipAddress, int port) : base(ipAddress, port)
        {
            _ServerRooms = new List<ServerRoom>();
        }
        protected override void OnProfileUpdateMessage(ServerClient client, ProfileUpdateMessage msg)
        {
            client.PlayerName = msg.PlayerName;
            client.PlayerColor = msg.PlayerColor;
            SendMessageToClient(client, msg);
        }
        protected override void OnRoomUpdateMessage(ServerClient client, RoomUpdateMessage msg)
        {
            switch (msg.UpdateStatus)
            {
                case RoomUpdateStatus.NewRoomRequest:
                    _ServerRooms.Add(new ServerRoom(msg));
                    SendMessageToClient(client, msg);
                    msg.UpdateStatus = RoomUpdateStatus.Broadcast;
                    BroadcastToClients(msg, client);
                    break;
                case RoomUpdateStatus.NewRoomRollback:
                    var local = _ServerRooms.Single(x => x.RoomID == msg.RoomID);
                    _ServerRooms.Remove(local);
                    SendMessageToClient(client, msg);
                    msg.UpdateStatus = RoomUpdateStatus.Broadcast;
                    BroadcastToClients(msg, client);
                    break;
                case RoomUpdateStatus.JoinRoomRequest:
                    local = _ServerRooms.Single(x => x.RoomID == msg.RoomID);
                    local.UpdateMessage.Player2ID = msg.Player2ID;
                    GameUpdateMessage gameMsg = local.GetGameUpdateMessage(this);
                    gameMsg.UpdateStatus = GameUpdateStatus.GameStarted;
                    gameMsg.TurnMove = PlayerMove.X;
                    SendMessageToClient(this[gameMsg.Player1ID], gameMsg);
                    SendMessageToClient(this[gameMsg.Player2ID], gameMsg);
                    msg.UpdateStatus = RoomUpdateStatus.Broadcast;
                    BroadcastToClients(msg);
                    break;
            }
        }
        protected override void OnGameUpdateMessage(ServerClient client, GameUpdateMessage msg)
        {
            switch (msg.UpdateStatus)
            {
                case GameUpdateStatus.PlayerMove:
                    var local = _ServerRooms.Single(x => x.RoomID == msg.RoomID);
                    local.UpdateGameBoard(msg.GameBoard);
                    bool winMove = CheckFour(local.GameBoard, msg.TurnMove);
                    int full = FullBoard(local.GameBoard);

                    GameUpdateMessage gameUpdate = local.GetGameUpdateMessage(this);
                    if (!winMove && full != 7)
                        gameUpdate.TurnMove = msg.TurnMove == PlayerMove.X ? PlayerMove.O : msg.TurnMove == PlayerMove.O ? PlayerMove.X : PlayerMove.N;
                    else
                        gameUpdate.TurnMove = PlayerMove.N;
                    gameUpdate.UpdateStatus = GameUpdateStatus.PlayerMove;
                    SendMessageToClient(this[gameUpdate.Player1ID], gameUpdate);
                    SendMessageToClient(this[gameUpdate.Player2ID], gameUpdate);

                    if (winMove)
                    {
                        if (msg.TurnMove == PlayerMove.X)
                        {
                            gameUpdate.UpdateStatus = GameUpdateStatus.PlayerWin;
                            SendMessageToClient(this[gameUpdate.Player1ID], gameUpdate);
                            gameUpdate.UpdateStatus = GameUpdateStatus.PlayerLose;
                            SendMessageToClient(this[gameUpdate.Player2ID], gameUpdate);
                        }
                        else if (msg.TurnMove == PlayerMove.O)
                        {
                            gameUpdate.UpdateStatus = GameUpdateStatus.PlayerLose;
                            SendMessageToClient(this[gameUpdate.Player1ID], gameUpdate);
                            gameUpdate.UpdateStatus = GameUpdateStatus.PlayerWin;
                            SendMessageToClient(this[gameUpdate.Player2ID], gameUpdate);
                        }
                    }
                    else if (full == 7)
                    {
                        gameUpdate.UpdateStatus = GameUpdateStatus.GameDraw;
                        SendMessageToClient(this[gameUpdate.Player1ID], gameUpdate);
                        SendMessageToClient(this[gameUpdate.Player2ID], gameUpdate);
                    }
                    break;
            }
        }

        private bool CheckFour(PlayerMove[,] board, PlayerMove activePlayer)
        {
            bool win = false;
            for (int i = 8; i >= 1; --i)
            {
                for (int ix = 9; ix >= 1; --ix)
                {
                    if (board[i, ix] == activePlayer &&
                        board[i - 1, ix - 1] == activePlayer &&
                        board[i - 2, ix - 2] == activePlayer &&
                        board[i - 3, ix - 3] == activePlayer)
                    {
                        win = true;
                    }
                    if (board[i, ix] == activePlayer &&
                        board[i, ix - 1] == activePlayer &&
                        board[i, ix - 2] == activePlayer &&
                        board[i, ix - 3] == activePlayer)
                    {
                        win = true;
                    }
                    if (board[i, ix] == activePlayer &&
                        board[i - 1, ix] == activePlayer &&
                        board[i - 2, ix] == activePlayer &&
                        board[i - 3, ix] == activePlayer)
                    {
                        win = true;
                    }
                    if (board[i, ix] == activePlayer &&
                        board[i - 1, ix + 1] == activePlayer &&
                        board[i - 2, ix + 2] == activePlayer &&
                        board[i - 3, ix + 3] == activePlayer)
                    {
                        win = true;
                    }
                    if (board[i, ix] == activePlayer &&
                         board[i, ix + 1] == activePlayer &&
                         board[i, ix + 2] == activePlayer &&
                         board[i, ix + 3] == activePlayer)
                    {
                        win = true;
                    }
                }
            }
            return win;
        }
        private int FullBoard(PlayerMove[,] board)
        {
            int full = 0;
            for (int i = 1; i <= 7; ++i)
            {
                var temp = board[1, i];
                if (temp == PlayerMove.O || temp == PlayerMove.X)
                    ++full;
            }
            return full;
        }
    }

    public class ServerRoom
    {
        public RoomUpdateMessage UpdateMessage { get; private set; }
        public int RoomID => UpdateMessage.RoomID;
        public PlayerMove[,] GameBoard { get; private set; }
        public ServerRoom(RoomUpdateMessage msg)
        {
            UpdateMessage = msg;
            UpdateMessage.RoomID = msg.Player1ID + 250;
            GameBoard = new PlayerMove[9, 10];
        }
        public GameUpdateMessage GetGameUpdateMessage(GameServer server)
        {
            GameUpdateMessage msg = new GameUpdateMessage();
            msg.RoomID = RoomID;
            msg.GameBoard = GameBoard;
            msg.Player1ID = UpdateMessage.Player1ID;
            msg.Player2ID = UpdateMessage.Player2ID;
            msg.Player1Name = server[UpdateMessage.Player1ID].PlayerName;
            msg.Player2Name = server[UpdateMessage.Player2ID].PlayerName;
            msg.Player1Color = server[UpdateMessage.Player1ID].PlayerColor;
            msg.Player2Color = server[UpdateMessage.Player2ID].PlayerColor;
            return msg;
        }
        public void UpdateGameBoard(PlayerMove[,] board)
        {
            GameBoard = board;
        }
    }
}
