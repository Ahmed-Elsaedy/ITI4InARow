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

        protected override void ProcessClientMessages(List<MessageBase> clientQueue)
        {
            base.ProcessClientMessages(clientQueue);
            foreach (MessageBase msg in clientQueue)
            {
                switch (msg.MsgType)
                {
                    case MessageType.RegisterMsg:
                        RegisterMessage clientMsg = (RegisterMessage)msg;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
