namespace ITI4InARow.Module.Core
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public class ProfileUpdateMessage : MessageBase
    {
        public ProfileUpdateMessage()
        {
            base.MsgType = MessageType.ProfileUpdateMessage;
        }

        public string Name { get; set; }
    }
}

