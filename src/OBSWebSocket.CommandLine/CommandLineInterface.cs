using System.CommandLine;
using OBSWebSocket.Client;

namespace OBSWebSocket.CommandLine;

public abstract class CommandLineInterface
{
    protected async Task<ObsClient> ConnectToObs(GlobalOptions globalOptions)
    {
        var obs = new ObsClient(new ObsClientOptions
        {
            HostName = globalOptions.HostName,
            Port = globalOptions.Port,
            Password = globalOptions.Password
        });

        await obs.Connect();

        if (!obs.Connected)
        {
            Console.Error.Write(
                "Could not connect to OBS. Please ensure that OBS is running, the OBS WebSocket server is enabled, and that you are properly authenticated.");
            Environment.Exit(-1);
            return null;
        }

        return obs;
    }

    protected abstract void RegisterCommands(RootCommand rootCommand, GlobalOptionsBinder globalOptionsBinder);

    public static void RegisterCommands<T>(RootCommand rootCommand, GlobalOptionsBinder globalOptionsBinder)
        where T : CommandLineInterface, new()
    {
        var cli = new T();
        cli.RegisterCommands(rootCommand, globalOptionsBinder);
    }
}