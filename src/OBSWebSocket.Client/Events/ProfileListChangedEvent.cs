using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class ProfileListChangedEvent : ObsEvent
{
    [JsonPropertyName("profiles")] 
    public string[] Profiles { get; set; } = Array.Empty<string>();
}