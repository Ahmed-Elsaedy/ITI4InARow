namespace ITI4InARow.Module.Core
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class GameUpdateMessage : MessageBase
    {
        public string[] viewSpectatorBoard;

        public int PlayerID { get; set; }
        public string Player2Color { get; set; }
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
                UpdateStatus = UpdateStatus,
                TokenPosition =TokenPosition,
                Player2Color = Player2Color
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
        lose,
        MakeYourMove,
        SpectatorJoin,
        viewMoveToSpectator,
        therisWinner
    }
}

