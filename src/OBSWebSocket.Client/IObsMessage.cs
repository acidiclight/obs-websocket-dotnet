namespace OBSWebSocket.Client;

public interface IObsMessage
{
    OpCode OpCode { get; }

    T? GetData<T>();
}