using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.DataTypes;

public class RecordStatus
{
    [JsonPropertyName("outputActive")]
    public bool OutputActive { get; set; }
    
    [JsonPropertyName("outputPaused")]
    public bool OutputPaused { get; set; }
    
    [JsonPropertyName("outputTimecode")]
    public string OutputTimecode { get; set; }
    
    [JsonPropertyName("outputDuration")]
    public float OutputDuration { get; set; }
    
    [JsonPropertyName("outputBytes")]
    public int OutputBytes { get; set; }
}

public class SavedOutput
{
    [JsonPropertyName("outputPath")]
    public string OutputPath { get; set; }
}