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
        protected override void ProcessServerMessages(List<MessageBase> serverQueue)
        {
            base.ProcessServerMessages(serverQueue);
            foreach (MessageBase msg in serverQueue)
            {
                switch (msg.MsgType)
                {
                    default:
                        break;
                }
            }
        }
    }
}
