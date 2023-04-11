using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class InputAudioBalanceChangedEvent : InputEvent
{
    [JsonPropertyName("inputAudioBalance")]
    public float InputAudioBalance { get; set; }
}