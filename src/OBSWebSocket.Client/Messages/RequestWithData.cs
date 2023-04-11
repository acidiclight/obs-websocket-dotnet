using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Messages;

public class RequestWithData<T> : Request
{
    [JsonPropertyName("requestData")]
    public T RequestData { get; set; }
}