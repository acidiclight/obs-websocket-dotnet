using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Messages;

public class Hello
{
    [JsonPropertyName("obsWebSocketVersion")]
    public string ObsWebSocketVersion { get; set; }
    
    [JsonPropertyName("rpcVersion")]
    public int RpcVersion { get; set; }
    
    [JsonPropertyName("authentication")]
    public AuthenticationChallenge? AuthenticationChallenge { get; set; }
}