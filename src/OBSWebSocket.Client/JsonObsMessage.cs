using System.Text.Json;

namespace OBSWebSocket.Client;

public class JsonObsMessage : IObsMessage
{
    private JsonElement data;
    private OpCode opCode;

    public OpCode OpCode => opCode;
    
    public JsonObsMessage(OpCode opCode, JsonElement data)
    {
        this.opCode = opCode;
        this.data = data;
    }

    public T? GetData<T>()
    {
        return data.Deserialize<T>();
    }
}