namespace ITI4InARow.Module.Core
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class RoomUpdateMessage : MessageBase
    {
        public RoomUpdateMessage()
        {
            base.MsgType = MessageType.RoomUpdateMessage;
        }

        public RoomUpdateMessage Copy() => 
            new RoomUpdateMessage { 
                RoomID = this.RoomID,
                Player1ID = this.Player1ID,
                Player2ID = this.Player2ID,
                UpdateState = this.UpdateState
            };

        public RoomStatus GetRoomStatus() => 
            (((this.Player1ID != 0) && (this.Player2ID == 0)) ? RoomStatus.Waiting : (((this.Player1ID != 0) && (this.Player2ID != 0)) ? RoomStatus.Gamming : RoomStatus.New));

        public int Player1ID { get; set; }

        public int Player2ID { get; set; }

        public int RoomID { get; set; }

        public RoomUpdateState UpdateState { get; set; }
    }
}

