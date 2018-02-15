using ITI4InARow.Module.Server;

namespace ITI4InARow.Module.Core
{
    public class MessageRevievedEventArgs
    {
        public ServerClient Client { get; set; }
        public MessageBase Message { get; private set; }
        public MessageRevievedEventArgs(MessageBase msg, ServerClient client)
        {
            Message = msg;
            Client = client;
        }
    }
}

