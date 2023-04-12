using System.Text.Json;
using System.Text.Json.Nodes;

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

    public JsonObject AsJson()
    {
        var jsonObject = new JsonObject();

        jsonObject.Add("op", (int)OpCode);
        jsonObject.Add("d", JsonObject.Create(data));
        
        return jsonObject;
    }
}