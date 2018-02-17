namespace ITI4InARow.Module.Core
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Drawing;
    

    public class ProfileUpdateMessage : MessageBase
    {
        public ProfileUpdateMessage()
        {
            MsgType = MessageType.ProfileUpdateMessage;
        }
        public string PlayerName { get; set; }
        public int PlayerColor { get; set; }
    }
}

