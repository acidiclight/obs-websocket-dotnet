using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Messages;

public class AuthenticationChallenge
{
    [JsonPropertyName("challenge")]
    public string Challenge { get; set; }
    
    [JsonPropertyName("salt")]
    public string Salt { get; set; }
}