namespace ITI4InARow.Module.Server
{
    using System;

    public enum ServerStatus
    {
        ServerStarted,
        ClientConnected,
        ListeningForClient,
        ReadingClientStream,
        SendingServerMessage,
        ClientDisconnected,
        ServerStopCancelled,
        ServerStopped,
        PendingForClients,
        StopWaitingForClients,
        ConnectionException,
        ProcessingIncommingMessage,
        IncommingClient,
        StartWaitingForClients,
        ReceivingKeepALiveFlag,
        SendingKeepALiveFlag
    }
}

