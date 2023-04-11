using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class InputVolumeChangedEvent : InputEvent
{
    [JsonPropertyName("inputVolumeMul")]
    public float InputVolumeMul { get; set; }
    
    [JsonPropertyName("inputVolumeDb")]
    public float InputVolumeDb { get; set; }
}