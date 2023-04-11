using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using OBSWebSocket.Client.DataTypes;

namespace OBSWebSocket.Client.Responses;

public class ScenesList
{
    [JsonPropertyName("currentProgramSceneName")]
    public string CurrentProgramSceneName { get; set; }
    
    [JsonPropertyName("currentPreviewSceneName")]
    public string? CurrentPreviewSceneName { get; set; }

    [JsonPropertyName("scenes")] 
    public SceneWithIndex[] Scenes { get; set; } = Array.Empty<SceneWithIndex>();
}