using System.Net.WebSockets;
using Websocket.Client;
using Websocket.Client.Models;

namespace OBSWebSocket.Client;
public class ObsClient : IDisposable
{
    private readonly ObsClientOptions options;
    private WebsocketClient? client;
    private bool connected = false;

    public bool Connected => client != null && client.IsRunning && client.IsStarted && connected;
    
    public ObsClient(ObsClientOptions? options = null)
    {
        options ??= new ObsClientOptions();
        this.options = options;
    }

    public async Task Connect()
    {
        var url = new Uri($"ws://{options.HostName}:{options.Port}/");

        client = new WebsocketClient(url, this.ClientFactory);
        client.Name = "OBS WebSocket";
        client.ReconnectTimeout = TimeSpan.FromSeconds(3);
        client.ErrorReconnectTimeout = TimeSpan.FromSeconds(3);
        client.DisconnectionHappened.Subscribe(OnDisconnected);
        client.MessageReceived.Subscribe(OnMessageReceived);
        client.ReconnectionHappened.Subscribe(OnReconnected);

        await client.Start();
    }

    private void OnReconnected(ReconnectionInfo info)
    {
        connected = true;
    }

    private void OnMessageReceived(ResponseMessage message)
    {
        if (message.MessageType == WebSocketMessageType.Binary)
        {
            OnBinaryReceived(message.Binary);
        }
        else
        {
            OnTextReceived(message.Text);
        }
    }

    private void OnTextReceived(string text)
    {
        Console.WriteLine(text);
    }

    private void OnBinaryReceived(byte[] data)
    {
        Console.WriteLine("[binary]");
    }

    private void OnDisconnected(DisconnectionInfo info)
    {
        connected = false;
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
