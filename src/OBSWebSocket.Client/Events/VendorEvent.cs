using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.Events;

public class VendorEvent : ObsEvent
{
    [JsonPropertyName("vendorName")]
    public string VendorName { get; set; }
    
    [JsonPropertyName("eventType")]
    public string EventType { get; set; }
    
    // TODO: Event Data. Welcome to C#, where we can't just have an object. It has to be a type otherwise it's painful to access.
}