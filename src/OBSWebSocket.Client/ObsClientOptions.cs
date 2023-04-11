namespace OBSWebSocket.Client;

public class ObsClientOptions
{
    public string HostName { get; set; } = "localhost";
    public ushort Port { get; set; } = 4455;
    public string? Password { get; set; }
}