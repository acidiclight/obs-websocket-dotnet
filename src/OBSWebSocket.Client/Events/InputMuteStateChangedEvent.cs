using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class InputMuteStateChangedEvent : InputEvent
{
    [JsonPropertyName("inputMuted")]
    public bool InputMuted { get; set; }
}