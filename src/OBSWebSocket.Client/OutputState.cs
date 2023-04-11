namespace OBSWebSocket.Client;

public enum OutputState
{
    Unknown,
    Starting,
    Started,
    Stopping,
    Stopped,
    Reconnecting,
    Reconnected,
    Paused,
    Resumed
}