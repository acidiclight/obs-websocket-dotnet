﻿using OBSWebSocket.Client;

namespace OBSWebSocket.CommandLine;

public class Program
{
    private static void Main(string[] args)
    {
        using var obs = new ObsClient();

        obs.Connect();

        if (!obs.Connected)
        {
            Console.Error.Write(
                "Could not connect to OBS. Please ensure that OBS is running, the OBS WebSocket server is enabled, and that you are properly authenticated.");
            return;
        }
        
        
    }
}