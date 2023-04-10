using System.Net.WebSockets;
using Websocket.Client;
using Websocket.Client.Models;

namespace OBSWebSocket.Client;
public class ObsClient : IDisposable
{
    private readonly ObsClientOptions options;
    private IWebsocketClient? client;

    public bool Connected => client != null && client.IsRunning && client.IsStarted;
    
    public ObsClient(ObsClientOptions? options = null)
    {
        options ??= new ObsClientOptions();
        this.options = options;
    }

    public void Connect()
    {
        var url = new Uri($"ws://{options.HostName}:{options.Port}/");

        client = new WebsocketClient(url, this.ClientFactory);
        client.Name = "OBS WebSocket";
        client.ReconnectTimeout = TimeSpan.FromSeconds(3);
        client.ErrorReconnectTimeout = TimeSpan.FromSeconds(3);
        client.MessageReceived.Subscribe(OnMessageReceived);
        client.ReconnectionHappened.Subscribe(OnReconnected);

        client.Start().Wait();
        Console.WriteLine("Jet the Hawk");
    }

    private void OnReconnected(ReconnectionInfo info)
    {
        Console.WriteLine("Reconnected");
    }

    private void OnMessageReceived(ResponseMessage message)
    {
        Console.WriteLine("Message received");
    }

    private void OnDisconnected(DisconnectionInfo info)
    {
        Console.WriteLine("Disconnected");
    }

    private ClientWebSocket ClientFactory()
    {
        var nativeClient = new ClientWebSocket
        {
            Options =
            {
                KeepAliveInterval = TimeSpan.FromSeconds(5)
            }
        };

        // TODO: Authentication.
        
        return nativeClient;
    }

    public void Dispose()
    {
        
    }
}

public class ObsClientOptions
{
    public string HostName { get; set; } = "localhost";
    public ushort Port { get; set; } = 4455;
    public string? Password { get; set; }

}
