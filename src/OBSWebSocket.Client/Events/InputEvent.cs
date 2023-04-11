using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public abstract class InputEvent : ObsEvent
{
    [JsonPropertyName("inputName")]
    public string InputName { get; set; }
}