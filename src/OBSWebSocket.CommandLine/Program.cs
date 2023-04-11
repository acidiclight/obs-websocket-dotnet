using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Invocation;
using System.CommandLine.NamingConventionBinder;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using OBSWebSocket.Client;

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

        var listScenesCommand = new Command("list-scenes",
            "List all available scenes. Each scene name is listed on a new line.");
        listScenesCommand.SetHandler(async (globalOptions) =>
        {
            await ListScenes(globalOptions);
        }, globalOptionsBinder);

        rootCommand.AddCommand(listScenesCommand);

        return await rootCommand.InvokeAsync(args);
    }

    private static async Task<ObsClient> Connect(GlobalOptions globalOptions)
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
    
    private static async Task ListScenes(GlobalOptions globalOptions)
    {
        using ObsClient obs = await Connect(globalOptions);
    }
}

public class GlobalOptions
{
    public string HostName { get; set; }
    public ushort Port { get; set; }
    public string? Password { get; set; }
}

public class GlobalOptionsBinder : BinderBase<GlobalOptions>
{
    private readonly Option<string> hostnameOption;
    private readonly Option<string?> passwordOption;
    private readonly Option<ushort> portOption;

    public GlobalOptionsBinder(Option<string> hostnameOption, Option<string?> passwordOption, Option<ushort> portOption)
    {
        this.hostnameOption = hostnameOption;
        this.portOption = portOption;
        this.passwordOption = passwordOption;

    }

    protected override GlobalOptions GetBoundValue(BindingContext bindingContext)
    {
        return new GlobalOptions()
        {
            HostName = bindingContext.ParseResult.GetValueForOption(hostnameOption) ?? "localhost",
            Port = bindingContext.ParseResult.GetValueForOption(portOption),
            Password = bindingContext.ParseResult.GetValueForOption(passwordOption)
        };
    }
}