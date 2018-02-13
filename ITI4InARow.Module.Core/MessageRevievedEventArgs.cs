namespace ITI4InARow.Module.Core
{
    public class MessageRevievedEventArgs
    {
        public int ClientHandle { get; private set; }
        public MessageBase Message { get; private set; }

        public MessageRevievedEventArgs(MessageBase msg, int clientHandle)
        {
            Message = msg;
            ClientHandle = clientHandle;
        }
    }
}

