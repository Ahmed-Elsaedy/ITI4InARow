using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ITI4InARow.Module.Core;

namespace ITI4InARow.Module.Server
{
    public class GameServer : ServerCore
    {
        public GameServer(byte[] ipAddress, int port)
            : base(ipAddress, port)
        {

        }
        protected override void ProcessClientMessage(ServerClient client, MessageBase msgBase)
        {
            msgBase.ClientID = client.ClientID;
            SendMessageToClient(client, msgBase);
        }
    }
}
