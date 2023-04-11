using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class CurrentSceneCollectionChangingEvent : ObsEvent
{
    [JsonPropertyName("sceneCollectionName")]
    public string SceneCollectionName { get; set; }
}