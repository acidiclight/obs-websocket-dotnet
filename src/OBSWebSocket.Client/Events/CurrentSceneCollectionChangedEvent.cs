using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class CurrentSceneCollectionChangedEvent : ObsEvent
{
    [JsonPropertyName("sceneCollectionName")]
    public string SceneCollectionName { get; set; }
}