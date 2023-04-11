using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class CurrentProfileChangedEvent : ObsEvent
{
    [JsonPropertyName("profileName")]
    public string ProfileName { get; set; }
}