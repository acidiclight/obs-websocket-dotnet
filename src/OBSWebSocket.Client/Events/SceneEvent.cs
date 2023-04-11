using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public abstract class SceneEvent : ObsEvent
{
    [JsonPropertyName("sceneName")]
    public string SceneName { get; set; }
}