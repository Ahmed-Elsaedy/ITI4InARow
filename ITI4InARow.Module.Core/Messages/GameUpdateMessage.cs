namespace ITI4InARow.Module.Core
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class GameUpdateMessage : MessageBase
    {
        public GameUpdateStatus UpdateStatus { get; set; }
        public PlayerMove[,] GameBoard { get; set; }
        public int RoomID { get; set; }
        public int Player1ID { get; set; }
        public int Player2ID { get; set; }
        public string Player1Name { get; set; }
        public string Player2Name { get; set; }
        public int Player1Color { get; set; }
        public int Player2Color { get; set; }
        public PlayerMove TurnMove { get; set; }
        public GameUpdateMessage()
        {
            MsgType = MessageType.GameUpdateMessage;
        }
        public string GetStatusString()
        {
            string retVal = string.Empty;
            retVal = $"{Player1Name} | {Player2Name}";
            if (TurnMove == PlayerMove.X)
                retVal = retVal.Replace(Player1Name, $"({Player1Name})");
            else if (TurnMove == PlayerMove.O)
                retVal = retVal.Replace(Player2Name, $"({Player2Name})");
            return retVal;
        }
        public int GetTurnID()
        {
            return TurnMove == PlayerMove.X ? Player1ID :
                TurnMove == PlayerMove.O ? Player2ID : 0;
        }
    }
    public enum GameUpdateStatus
    {
        GameStarted,
        PlayerMove,
        PlayerLose,
        PlayerWin,
        GameDraw,
    }
}

