using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class InputActiveStateChangedEvent : InputEvent
{
    [JsonPropertyName("videoActive")]
    public bool VideoActive { get; set; }
}