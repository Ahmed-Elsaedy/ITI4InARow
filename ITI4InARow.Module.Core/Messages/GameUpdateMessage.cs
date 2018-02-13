namespace ITI4InARow.Module.Core
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class GameUpdateMessage : MessageBase
    {
        public int PlayerID { get; set; }
        public int RoomID { get; set; }
        public int GameMove { get; set; }
        public GameUpdateStatus UpdateStatus { get; set; }
        public int TokenPosition { get; set; }
        public bool IsGameRunning { get; set; }

        public GameUpdateMessage()
        {
            MsgType = MessageType.GameUpdateMessage;
        }
        public GameUpdateMessage Copy() =>
            new GameUpdateMessage
            {
                RoomID = RoomID,
                PlayerID = PlayerID,
                UpdateStatus = UpdateStatus
            };
    }
    public enum GameUpdateStatus
    {
        GameStarted,
        PlayerMove,
        GameEnded,
        GameLeave,
        GameDraw,
        win,
        lose
    }
}

