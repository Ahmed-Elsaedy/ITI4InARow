﻿namespace ITI4InARow.Module.Client
{
    using ITI4InARow.Module.Core;
    using System;

    public class GameClient : ClientCore
    {
        public int ClientID { get; private set; }
        public string NickName { get; private set; }
        public string UserColor { get; private set; }
        public string PreferedColor { get; private set; }

        protected override void OnProfileUpdateMessage(ProfileUpdateMessage msg)
        {
            ClientID = msg.ClientID;
            NickName = msg.Name;
            UserColor = msg.UserColor;
            PreferedColor = msg.UserColor;
        }
        protected override void OnRoomUpdateMessage(RoomUpdateMessage msg)
        {
            RoomUpdateMessage?.Invoke(this, msg);
        }
        protected override void OnGameUpdateMessage(GameUpdateMessage msg)
        {
            GameUpdateMessage?.Invoke(this, msg);
        }

        public event EventHandler<GameUpdateMessage> GameUpdateMessage;
        public event EventHandler<RoomUpdateMessage> RoomUpdateMessage;
    }
}

