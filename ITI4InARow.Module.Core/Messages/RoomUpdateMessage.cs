namespace ITI4InARow.Module.Core
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class RoomUpdateMessage : MessageBase
    {
        public int Player1ID { get; set; }
        public int Player2ID { get; set; }
        public int RoomID { get; set; }
        public RoomUpdateState UpdateState { get; set; }

        public string[] viewspac;
        public RoomUpdateMessage()
        {
            MsgType = MessageType.RoomUpdateMessage;
        }
        public RoomUpdateMessage Copy() =>
            new RoomUpdateMessage
            {
                RoomID = RoomID,
                Player1ID = Player1ID,
                Player2ID = Player2ID,
                UpdateState = UpdateState
            };
        public RoomStatus GetRoomStatus() =>
            (((Player1ID != 0) && (Player2ID == 0)) ? RoomStatus.Waiting :
            (((Player1ID != 0) && (Player2ID != 0)) ? RoomStatus.Gamming : RoomStatus.New));
    }
    public enum RoomStatus
    {
        New,
        Waiting,
        Gamming
    }
    public enum RoomUpdateState
    {
        NewRoomRequest,
        NewRoomRollback,
        Player2Connected,
        Broadcast,
        RoomComplete,
        newSpectatorReq
    }
}

