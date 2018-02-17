using ITI4InARow.Module.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI4InARow.Module.Client
{
    public class GameClient : ClientCore
    {
        public int PlayerID { get; private set; }
        public string PlayerName { get; private set; }
        public int PlayerColor { get; private set; }
        public List<RoomUpdateMessage> RoomsUpdates { get; private set; }
        public GameUpdateMessage GameUpdate { get; set; }

        public GameClient()
        {
            RoomsUpdates = new List<RoomUpdateMessage>();
        }
        protected override void OnProfileUpdateMessage(ProfileUpdateMessage msg)
        {
            PlayerID = msg.ClientID;
            PlayerName = msg.PlayerName;
            PlayerColor = msg.PlayerColor;
            ProfileUpdateMessage?.Invoke(this, msg);
        }
        protected override void OnRoomUpdateMessage(RoomUpdateMessage msg)
        {
            switch (msg.UpdateStatus)
            {
                case RoomUpdateStatus.NewRoomRequest:
                    RoomsUpdates.Add(msg);
                    break;
                case RoomUpdateStatus.NewRoomRollback:
                    RoomsUpdates.Remove(RoomsUpdates.Single(x => x.RoomID == msg.RoomID));
                    break;
                case RoomUpdateStatus.Broadcast:
                    RoomUpdateMessage message = RoomsUpdates.SingleOrDefault(x => x.RoomID == msg.RoomID);
                    if (message == null && msg.GetRoomStatus() != RoomStatus.Obsolete)
                        RoomsUpdates.Add(msg);
                    else if (message != null && msg.GetRoomStatus() == RoomStatus.Obsolete)
                        RoomsUpdates.Remove(message);
                    else if (message != null && msg.GetRoomStatus() != RoomStatus.Obsolete)
                        RoomsUpdates[RoomsUpdates.IndexOf(message)] = msg;
                    break;
            }
            RoomUpdateMessage?.Invoke(this, msg);
        }
        protected override void OnGameUpdateMessage(GameUpdateMessage msg)
        {
            GameUpdate = msg;
            GameUpdateMessage?.Invoke(this, msg);
        }

        public event EventHandler<ProfileUpdateMessage> ProfileUpdateMessage;
        public event EventHandler<RoomUpdateMessage> RoomUpdateMessage;
        public event EventHandler<GameUpdateMessage> GameUpdateMessage;
    }
}
