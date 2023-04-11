using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Messages;

public class RequestStatus
{
    [JsonPropertyName("result")]
    public bool Result { get; set; }
    [JsonPropertyName("code")]
    public int Code { get; set; }
    [JsonPropertyName("comment")]
    public string? Comment { get; set; }
}