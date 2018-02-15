namespace ITI4InARow.Module.Core
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class MessageBase
    {
        public int ClientID { get; set; }

        public MessageType MsgType { get; set; }
        public MessageBase()
        {
            MsgType = MessageType.MessageBase;
        }
    }
    public enum MessageType
    {
        MessageBase,
        ProfileUpdateMessage,
        RoomUpdateMessage,
        GameUpdateMessage
    }
}

