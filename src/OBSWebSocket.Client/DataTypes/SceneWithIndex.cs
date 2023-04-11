using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.DataTypes;

public class SceneWithIndex : Scene
{
    [JsonPropertyName("sceneIndex")]
    public int SceneIndex { get; set; }
}