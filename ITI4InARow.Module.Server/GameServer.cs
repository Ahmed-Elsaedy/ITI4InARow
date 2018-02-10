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

        }
        protected override void ProcessClientMessage(ServerClient client, MessageBase msgBase)
        {
            //Client Req handling 
            switch (msgBase.MsgType.Name)
            {
                case "RegisterMessage":
                    ListofRoomsMessage ListOfAvalableRooms = new ListofRoomsMessage(availableRooms);
                    SendMessageToClient(client, ListOfAvalableRooms);
                    break;

                default:
                    break;
            }

            //SendMessageToClient(client, my message object  );

        }

        protected override void OnRegisterMessage(RegisterMessage msg)
        {
            ListofRoomsMessage ListOfAvalableRooms = new ListofRoomsMessage(availableRooms);
            SendMessageToClient(client, ListOfAvalableRooms);
        }
    }
}
