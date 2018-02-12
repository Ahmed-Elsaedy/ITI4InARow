namespace ITI4InARow.Module.Core
{
    using System;

    public enum RoomUpdateState
    {
        NewRoomRequest,
        NewRoomRollback,
        Player2Connected,
        Broadcast,
        RoomComplete
    }
}

