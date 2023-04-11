using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Messages;

public class Identify
{
    [JsonPropertyName("authentication")]
    public string? AuthenticationString { get; set; }
    
    [JsonPropertyName("rpcVersion")]
    public int RpcVersion { get; set; }
}

public class RequestResponse<T> : RequestResponse
{
    [JsonPropertyName("responseData")]
    public T ResponseData { get; set; }
}