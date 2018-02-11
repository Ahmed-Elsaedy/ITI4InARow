using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITI4InARow.Module.Core;
using ITI4InARow.Game.Client;

namespace ITI4InARow.Module.Server
{
    public class GameServer : ServerCore
    {
        List<ServerRoom> availableRooms;
        public GameServer(byte[] ipAddress, int port)
            : base(ipAddress, port)
        {
            availableRooms = new List<ServerRoom>();
            ServerRoom testroom = new ServerRoom();
            //testroom.name = "mrwan";
            //testroom.isSpectator = true;
            //testroom.RoomID = 0;
            //testroom.roomState = RoomState.wating;
            //testroom.spectatorCount = 0;
            //availableRooms.Add(testroom);
        }
        protected override void OnRegisterMessage(ServerClient client, RegisterMessage msg)
        {
            ListofRoomsMessage ListOfAvalableRooms = new ListofRoomsMessage(availableRooms);
            SendMessageToClient(client, ListOfAvalableRooms);
        }
    }
}
