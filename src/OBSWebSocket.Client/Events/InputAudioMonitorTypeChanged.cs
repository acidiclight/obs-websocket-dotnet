using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class InputAudioMonitorTypeChanged : InputEvent
{
    [JsonPropertyName("monitorType")]
    public string MonitorType { get; set; }
}