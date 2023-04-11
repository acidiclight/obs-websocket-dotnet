using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class SceneCreatedEvent : SceneEvent
{
    [JsonPropertyName("isGroup")]
    public bool IsGroup { get; set; }
}