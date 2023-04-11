using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class InputShowStateChangedEvent : InputEvent
{
    [JsonPropertyName("videoShowing")]
    public bool VideoShowing { get; set; }
}