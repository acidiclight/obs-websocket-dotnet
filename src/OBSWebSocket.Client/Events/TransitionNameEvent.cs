using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public abstract class TransitionNameEvent : TransitionEvent
{
    [JsonPropertyName("transitionName")]
    public string TransitionName { get; set; }
}