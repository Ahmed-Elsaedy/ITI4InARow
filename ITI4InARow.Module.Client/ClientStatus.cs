namespace ITI4InARow.Module.Client
{
    using System;

    public enum ClientStatus
    {
        ConnectionError,
        ClientConnected,
        ClientDisconnected,
        ConnectionException,
        ListeningForServer,
        ReadingServerStream,
        ProcessingIncommingMessage,
        SendingClientMessage,
        SendingKeepALiveFlag,
        ReceivingKeepALiveFlag
    }
}

