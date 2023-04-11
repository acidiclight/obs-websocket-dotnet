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

public class AuthenticationChallenge
{
    [JsonPropertyName("challenge")]
    public string Challenge { get; set; }
    
    [JsonPropertyName("salt")]
    public string Salt { get; set; }
}

public class Identify
{
    [JsonPropertyName("authentication")]
    public string? AuthenticationString { get; set; }
    
    [JsonPropertyName("rpcVersion")]
    public int RpcVersion { get; set; }
}