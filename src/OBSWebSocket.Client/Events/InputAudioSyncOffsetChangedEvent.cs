using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class InputAudioSyncOffsetChangedEvent : InputEvent
{
    [JsonPropertyName("inputAudioSyncOffset")]
    public float InputAudioSyncOffset { get; set; }
}