using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class SceneNameChangedEvent : SceneEvent
{
    [JsonPropertyName("oldSceneName")]
    public string OldSceneName { get; set; }
}