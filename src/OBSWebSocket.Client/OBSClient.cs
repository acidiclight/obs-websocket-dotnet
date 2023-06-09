﻿using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Nodes;
using OBSWebSocket.Client.DataTypes;
using OBSWebSocket.Client.Messages;
using OBSWebSocket.Client.Responses;
using Websocket.Client;
using Websocket.Client.Models;

namespace OBSWebSocket.Client;
public class ObsClient : IDisposable
{
    private readonly ObsClientOptions options;
    private readonly Queue<IObsMessage> messageReceiveQueue = new Queue<IObsMessage>();
    private readonly ManualResetEvent messagesReceived = new ManualResetEvent(false);
    private readonly int ourRpcVersion = 1;
    private WebsocketClient? client;
    private bool connected = false;
    

    public bool Connected => client != null && client.IsRunning && client.IsStarted && connected;
    
    public ObsClient(ObsClientOptions? options = null)
    {
        options ??= new ObsClientOptions();
        this.options = options;
    }

    public async Task SetCurrentProgramScene(string sceneName)
    {
        ThrowIfNotConnected();

        await SendRequest(new RequestWithData<Scene>
        {
            RequestType = "SetCurrentProgramScene",
            RequestData = new Scene()
            {
                SceneName = sceneName
            }
        });

        await WaitForResponse();
    }

    public async Task StartRecording()
    {
        ThrowIfNotConnected();
        
        await SendRequest(new SimpleRequest
        {
            RequestType = "StartRecord"
        });

        await WaitForResponse();
    }
    
    public async Task ToggleRecording()
    {
        ThrowIfNotConnected();
        
        await SendRequest(new SimpleRequest
        {
            RequestType = "ToggleRecord"
        });

        await WaitForResponse();
    }

    public async Task<RequestResponse<InputList>> GetInputList(string? inputKind = null)
    {
        ThrowIfNotConnected();

        await SendRequest(new RequestWithData<InputListRequest>
        {
            RequestType = "GetInputList",
            RequestData = new InputListRequest
            {
                InputKind = inputKind
            }
        });

        return await WaitForResponse<InputList>();
    }
    
    public async Task<RequestResponse<SavedOutput>> StopRecording()
    {
        ThrowIfNotConnected();
        
        await SendRequest(new SimpleRequest
        {
            RequestType = "StopRecord"
        });

        return await WaitForResponse<SavedOutput>();
    }

    public async Task ToggleRecordPause()
    {
        ThrowIfNotConnected();
        
        await SendRequest(new SimpleRequest
        {
            RequestType = "ToggleRecordPause"
        });

        await WaitForResponse();
    }

    public async Task PauseRecording()
    {
        ThrowIfNotConnected();
        
        await SendRequest(new SimpleRequest
        {
            RequestType = "PauseRecord"
        });

        await WaitForResponse();
    }
    
    public async Task ResumeRecording()
    {
        ThrowIfNotConnected();
        
        await SendRequest(new SimpleRequest
        {
            RequestType = "ResumeRecord"
        });

        await WaitForResponse();
    }
    
    public async Task<RequestResponse<ScenesList>> GetScenesList()
    {
        ThrowIfNotConnected();

        await SendRequest(new SimpleRequest
        {
            RequestType = "GetSceneList"
        });

        return await WaitForResponse<ScenesList>();
    }

    public async Task<RequestResponse<RecordStatus>> GetRecordStatus()
    {
        ThrowIfNotConnected();

        await SendRequest(new SimpleRequest
        {
            RequestType = "GetRecordStatus"
        });

        return await WaitForResponse<RecordStatus>();
    }
    
    public async Task Connect()
    {
        var url = new Uri($"ws://{options.HostName}:{options.Port}/");

        client = new WebsocketClient(url, this.ClientFactory);
        client.Name = "OBS WebSocket";
        client.ReconnectTimeout = TimeSpan.FromSeconds(3);
        client.ErrorReconnectTimeout = TimeSpan.FromSeconds(3);
        client.DisconnectionHappened.Subscribe(OnDisconnected);
        client.MessageReceived.Subscribe(OnMessageReceived);
        client.ReconnectionHappened.Subscribe(OnReconnected);

        await client.Start();

        await PerformConnectionFlow();
    }

    private async Task<RequestResponse<T>> WaitForResponse<T>()
    {
        IObsMessage message = await WaitForMessageWithOpCode(OpCode.RequestResponse);

        return message.GetData<RequestResponse<T>>();
    }

    private async Task WaitForResponse()
    {
        await WaitForMessageWithOpCode(OpCode.RequestResponse);
        
        // TODO: Error handling.
    }
    
    private async Task SendRequest(Request request)
    {
        await Task.Run(() =>
        {
            SendMessage(OpCode.Request, request);
        });
    }
    
    private void ThrowIfNotConnected()
    {
        if (!Connected)
            throw new InvalidOperationException("Not connected to OBS.");
    }
    
    private async Task PerformConnectionFlow()
    {
        Hello? hello = await WaitForData<Hello>(OpCode.Hello);

        // We MUST get a valid Hello object.
        if (hello == null)
        {
            connected = false;
            return;
        }



        var identify = new Identify()
        {
            RpcVersion = ourRpcVersion
        };
        
        if (hello.AuthenticationChallenge != null)
        {
            if (string.IsNullOrWhiteSpace(options.Password))
            {
                connected = false;
                throw new InvalidOperationException(
                    "The OBS WebSocket server is configured to require authentication, but no password has been provided.");
            }

            string challengeResponse = Helpers.GenerateAuthenticationString(options.Password,
                hello.AuthenticationChallenge.Challenge, hello.AuthenticationChallenge.Salt);

            identify.AuthenticationString = challengeResponse;
        }

        SendMessage(OpCode.Identify, identify);
        
        // We need to wait for an "Identified" message. If we get anything else, it means authentication failed.
        // Technically, the server will drop us if that happens. Which means, behind the scenes, we reconnect.
        // That means OBS will send us another Hello message.
        IObsMessage identifiedResponse = await WaitForMessage();
        if (identifiedResponse.OpCode != OpCode.Identified)
        {
            // Fail silently.
            connected = false;
            return;
        }
    }

    private void SendMessage(OpCode opCode, object messageContents)
    {
        if (client == null || !Connected)
            throw new InvalidOperationException("Not connected");
            
        // Step 1. Serialize the data object as JSON.
        string dataJson = JsonSerializer.Serialize(messageContents);
        
        // Step 2: Deserialize it into a JSON DOM object
        var data = JsonObject.Parse(dataJson);
        
        // Step 3: Create the outer JSON structure containing the opCode.
        JsonObject outerStructure = new JsonObject();
        outerStructure.Add("d", data);
        outerStructure.Add("op", (int)opCode);
        
        // Finally, send it to the server.
        string jsonText = JsonSerializer.Serialize(outerStructure);

        client.Send(jsonText);
    }

    private async Task<IObsMessage> WaitForMessageWithOpCode(OpCode opCode)
    {
        IObsMessage message = await WaitForMessage();

        if (message.OpCode != opCode)
            throw new Exception("Server sent a message with an unexpected opcode.");

        return message;
    }
    
    private async Task<T?> WaitForData<T>(OpCode opCode)
    {
        return (await WaitForMessageWithOpCode(opCode)).GetData<T>();
    }
    
    private void OnReconnected(ReconnectionInfo info)
    {
        connected = true;
    }

    private async Task<IObsMessage> WaitForMessage()
    {
        await Task.Run(messagesReceived.WaitOne);

        IObsMessage result = messageReceiveQueue.Dequeue();

        if (messageReceiveQueue.Count == 0)
            messagesReceived.Reset();

        return result;
    }

    private void OnMessageReceived(ResponseMessage message)
    {
        if (message.MessageType == WebSocketMessageType.Binary)
        {
            OnBinaryReceived(message.Binary);
        }
        else
        {
            OnTextReceived(message.Text);
        }
    }

    private void OnTextReceived(string text)
    {
        using var doc = JsonDocument.Parse(text);

        JsonElement root = doc.RootElement;
        if (root.ValueKind != JsonValueKind.Object)
            throw new NotSupportedException("Malformed message received from server");
        
        if (!root.TryGetProperty("op", out JsonElement opCodeProperty))
            throw new NotSupportedException("Malformed message received from server");
        
        if (!root.TryGetProperty("d", out JsonElement dataProperty))
            throw new NotSupportedException("Malformed message received from server");
        
        if (!opCodeProperty.TryGetInt32(out int rawOpCode))
            throw new NotSupportedException("Malformed message received from server");

        OpCode opCode = (OpCode)rawOpCode;

        var message = new JsonObsMessage(opCode, dataProperty.Clone());

        // Events are handled differently.
        if (message.OpCode == OpCode.Event) 
            ProcessEvent(message);
        
        messageReceiveQueue.Enqueue(message);
        messagesReceived.Set();
    }

    private void OnBinaryReceived(byte[] data)
    {
        Console.WriteLine("[binary]");
    }

    private void OnDisconnected(DisconnectionInfo info)
    {
        connected = false;
    }

    private ClientWebSocket ClientFactory()
    {
        var nativeClient = new ClientWebSocket
        {
            Options =
            {
                KeepAliveInterval = TimeSpan.FromSeconds(5)
            }
        };

        // TODO: Authentication.
        
        return nativeClient;
    }

    private void ProcessEvent(IObsMessage message)
    {
        
    }
    
    public void Dispose()
    {
        
    }

    public async Task<JsonObject> SendRaw(JsonObject json)
    {
        ThrowIfNotConnected();
        
        client?.Send(json.ToString());

        var message = await WaitForMessage();

        return message.AsJson();
    }
    
}