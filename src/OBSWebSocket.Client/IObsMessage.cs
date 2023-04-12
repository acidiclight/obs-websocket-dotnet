using System.Text.Json.Nodes;

namespace OBSWebSocket.Client;

public interface IObsMessage
{
    OpCode OpCode { get; }

    T? GetData<T>();

    JsonObject AsJson();
}