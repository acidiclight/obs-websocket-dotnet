using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Messages;

public abstract class Request
{
    [JsonPropertyName("requestType")]
    public string RequestType { get; set; }

    [JsonPropertyName("requestId")]
    public string RequestId { get; set; } = Guid.NewGuid().ToString();
}