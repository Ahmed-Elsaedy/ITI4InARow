namespace ITI4InARow.Module.Core
{
    public class RoomUpdateMessage : MessageBase
    {
        public int RoomID { get; set; }
        public string RoomName { get; set; }
        public int RoomPass { get; set; }
        public int Player1ID { get; set; }
        public int Player2ID { get; set; }
        public RoomUpdateStatus UpdateStatus { get; set; }

        public RoomUpdateMessage()
        {
            MsgType = MessageType.RoomUpdateMessage;
        }
        public RoomStatus GetRoomStatus()
        {
            if (RoomID == 0)
                return RoomStatus.New;
            else if (Player1ID == 0 && Player2ID == 0)
                return RoomStatus.Obsolete;
            else if (Player1ID != 0 && Player2ID == 0)
                return RoomStatus.Waiting;
            else
                return RoomStatus.Gamming;
        }
        public override string ToString()
        {
            return $"{RoomID}:{RoomName} - {GetRoomStatus().ToString()}";
        }
    }
    public enum RoomStatus
    {
        New,
        Obsolete,
        Waiting,
        Gamming
    }
    public enum RoomUpdateStatus
    {
        NewRoomRequest,
        NewRoomRollback,
        Broadcast,
        JoinRoomRequest
    }
}

