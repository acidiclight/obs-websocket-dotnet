using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class CurrentSceneTransitionDurationChanged : TransitionEvent
{
    [JsonPropertyName("transitionDuration")]
    public float TransitionDuration { get; set; }
}