using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class InputCreatedEvent : InputEvent
{
    [JsonPropertyName("inputKind")]
    public string InputKind { get; set; }
    
    [JsonPropertyName("unversionedInputKind")]
    public string UnversionedInputKind { get; set; }
    
    // TODO: inputSettings
    // TODO: defaultInputSettings
}