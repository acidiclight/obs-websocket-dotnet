using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.DataTypes;

public class Scene
{
    [JsonPropertyName("sceneName")]
    public string SceneName { get; set; }
}