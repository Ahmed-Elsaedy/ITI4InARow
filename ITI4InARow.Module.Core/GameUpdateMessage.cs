namespace ITI4InARow.Module.Core
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class GameUpdateMessage : MessageBase
    {
        public GameUpdateMessage()
        {
            base.MsgType = MessageType.GameUpdateMessage;
        }

        public GameUpdateMessage Copy() => 
            new GameUpdateMessage { 
                RoomID = this.RoomID,
                PlayerID = this.PlayerID,
                UpdateStatus = this.UpdateStatus
            };

        public int PlayerID { get; set; }

        public int RoomID { get; set; }

        public int GameMove { get; set; }

        public GameUpdateStatus UpdateStatus { get; set; }
    }
}

