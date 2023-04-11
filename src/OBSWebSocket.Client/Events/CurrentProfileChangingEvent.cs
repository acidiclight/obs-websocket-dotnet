using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class CurrentProfileChangingEvent : ObsEvent
{
    [JsonPropertyName("profileName")]
    public string ProfileName { get; set; }
}