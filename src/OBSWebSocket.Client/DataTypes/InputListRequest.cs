using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace OBSWebSocket.Client.DataTypes;

public class InputListRequest
{
    [JsonPropertyName("inputKind")]
    public string? InputKind { get; set; }
}

public class InputList
{
    [JsonPropertyName("inputs")]
    public Input[] Inputs { get; set; } = Array.Empty<Input>();
}

public class Input
{
    [JsonPropertyName("inputKind")]
    public string InputKind { get; set; }
    
    [JsonPropertyName("inputName")]
    public string InputName { get; set; }
    
    [JsonPropertyName("unversionedInputKind")]
    public string UnversionedInputKind { get; set; }
}