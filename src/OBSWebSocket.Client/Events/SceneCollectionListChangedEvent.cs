using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class SceneCollectionListChangedEvent : ObsEvent
{
    [JsonPropertyName("sceneCollections")]
    public string[] SceneCollections { get; set; } = Array.Empty<string>();
}