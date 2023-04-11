using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class SceneListChangedEvent : ObsEvent
{
    [JsonPropertyName("scenes")]
    public string[] Scenes { get; set; } = Array.Empty<string>();
}