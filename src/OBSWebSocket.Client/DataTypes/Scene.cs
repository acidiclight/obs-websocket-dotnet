using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.DataTypes;

public class Scene
{
    [JsonPropertyName("sceneIndex")]
    public int SceneIndex { get; set; }
    
    [JsonPropertyName("sceneName")]
    public string SceneName { get; set; }
}