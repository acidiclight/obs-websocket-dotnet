using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class InputNameChangedEvent : InputEvent
{
    [JsonPropertyName("oldInputName")]
    public string OldInputName { get; set; }
}