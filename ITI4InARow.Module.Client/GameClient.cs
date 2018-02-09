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
        public GameClient()
        {

        }
        protected override void ProcessServerMessage(MessageBase msgBase)
        {
            string msgType = msgBase.MsgType.Name;
            switch (msgType)
            {
                case "ListofRoomsMessage":
                    ListofRoomsMessage AvalableRooms = (ListofRoomsMessage)msgBase;
                    //now you have here the list of all the rooms in server 

                    break;
                default:
                    break;
            }
        }
    }
}
