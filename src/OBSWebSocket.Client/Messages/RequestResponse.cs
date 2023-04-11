using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Messages;

public class RequestResponse
{
    [JsonPropertyName("requestType")]
    public string RequestType { get; set; }
    
    [JsonPropertyName("requestId")]
    public string RequestId { get; set; }
    
    [JsonPropertyName("requestStatus")]
    public RequestStatus RequestSTatus { get; set; }
}