namespace ITI4InARow.Module.Client
{
    using ITI4InARow.Module.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GameClient : ClientCore
    {
        public int PlayerID { get; private set; }
        public string PlayerName { get; private set; }
        public string PlayerColor { get; private set; }

        public RoomUpdateMessage LastRoomUpdateMsg { get; private set; }
        public GameUpdateMessage LastGameUpdateMsg { get; private set; }

        

        protected override void OnProfileUpdateMessage(ProfileUpdateMessage msg)
        {
            PlayerID = msg.ClientID;
            PlayerName = msg.PlayerName;
            PlayerColor = msg.PlayerColor;
        }
        protected override void OnRoomUpdateMessage(RoomUpdateMessage msg)
        {
            switch (msg.UpdateState)
            {

            }
            RoomUpdateMessage?.Invoke(this, msg);
            LastRoomUpdateMsg = msg;
        }
        protected override void OnGameUpdateMessage(GameUpdateMessage msg)
        {
            GameUpdateMessage?.Invoke(this, msg);
            LastGameUpdateMsg = msg;
        }

        public event EventHandler<GameUpdateMessage> GameUpdateMessage;
        public event EventHandler<RoomUpdateMessage> RoomUpdateMessage;
    }
}

