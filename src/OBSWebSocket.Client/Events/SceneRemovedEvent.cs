using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class SceneRemovedEvent : SceneEvent
{
    [JsonPropertyName("isGroup")]
    public bool IsGroup { get; set; }
}