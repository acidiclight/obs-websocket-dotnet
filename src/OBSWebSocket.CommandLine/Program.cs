using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using OBSWebSocket.Client.Responses;

namespace OBSWebSocket.CommandLine;

public class Program
{
    private static readonly string description = "Command-line interface for controlling OBS Studio using OBS WebSockets";

    private static async Task<int> Main(params string[] args)
    {
        var hostnameOption = new Option<string>(
            aliases: new[] { "-H", "--hostname", "--host" },
            description: "The hostname to connect to. Defaults to localhost.",
            getDefaultValue: () => "localhost"
        );

        var passwordOption = new Option<string?>(
            aliases: new[] { "--password" },
            description:
            "If authentication is required by the OBS WebSocket server, this option may be used to specify the password.",
            getDefaultValue: () => string.Empty
        );

        var portOption = new Option<ushort>(
            aliases: new[] { "-P", "--port" },
            description: "The port to connect to. Defaults to 4455.",
            getDefaultValue: () => 4455
        );

        var globalOptionsBinder = new GlobalOptionsBinder(hostnameOption, passwordOption, portOption);
        
        var rootCommand = new RootCommand(description)
        {
            TreatUnmatchedTokensAsErrors = true
        };

        rootCommand.AddGlobalOption(hostnameOption);
        rootCommand.AddGlobalOption(passwordOption);
        rootCommand.AddGlobalOption(portOption);

        CommandLineInterface.RegisterCommands<ScenesCommands>(rootCommand, globalOptionsBinder);
        CommandLineInterface.RegisterCommands<RecordingCommands>(rootCommand, globalOptionsBinder);
        CommandLineInterface.RegisterCommands<InputsCommands>(rootCommand, globalOptionsBinder);
        CommandLineInterface.RegisterCommands<SendCommands>(rootCommand, globalOptionsBinder);

        return await rootCommand.InvokeAsync(args);
    }
}