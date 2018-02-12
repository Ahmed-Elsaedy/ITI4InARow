namespace ITI4InARow.Module.Client
{
    using ITI4InARow.Module.Core;
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class GameClient : ClientCore
    {
        public event EventHandler<ITI4InARow.Module.Core.GameUpdateMessage> GameUpdateMessage;
        public event EventHandler<ITI4InARow.Module.Core.RoomUpdateMessage> RoomUpdateMessage;

        protected override void OnGameUpdateMessage(ITI4InARow.Module.Core.GameUpdateMessage msg)
        {
            if (this.GameUpdateMessage != null)
            {
                EventHandler<ITI4InARow.Module.Core.GameUpdateMessage> gameUpdateMessage = this.GameUpdateMessage;
                gameUpdateMessage(this, msg);
            }
            else
            {
                EventHandler<ITI4InARow.Module.Core.GameUpdateMessage> expressionStack_A_0 = this.GameUpdateMessage;
            }
        }

        protected override void OnRegisterMessage(ProfileUpdateMessage msg)
        {
            this.ClientID = msg.ClientID;
            this.NickName = msg.Name;
        }

        protected override void OnRoomUpdateMessage(ITI4InARow.Module.Core.RoomUpdateMessage msg)
        {
            if (this.RoomUpdateMessage != null)
            {
                EventHandler<ITI4InARow.Module.Core.RoomUpdateMessage> roomUpdateMessage = this.RoomUpdateMessage;
                roomUpdateMessage(this, msg);
            }
            else
            {
                EventHandler<ITI4InARow.Module.Core.RoomUpdateMessage> expressionStack_A_0 = this.RoomUpdateMessage;
            }
        }

        public int ClientID { get; private set; }

        public string NickName { get; private set; }
    }
}

