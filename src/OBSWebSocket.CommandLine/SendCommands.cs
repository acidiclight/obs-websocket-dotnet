using System.CommandLine;
using System.Text.Json.Nodes;

namespace OBSWebSocket.CommandLine;

public class SendCommands : CommandLineInterface
{
    protected override void RegisterCommands(RootCommand rootCommand, GlobalOptionsBinder globalOptionsBinder)
    {
        var dataArgument = new Argument<string>("json", () => string.Empty,
            "Specifies the JSON data to send. If nothing is supplied, the data will be read from standard input instead.");
            
        var sendCommand = new Command("send", "Send raw JSON requests to OBS and print the JSON response.");

        sendCommand.AddArgument(dataArgument);
        
        sendCommand.SetHandler(async (globalOptions, json) =>
        {
            await Send(globalOptions, json);
        }, globalOptionsBinder, dataArgument);
        
        rootCommand.AddCommand(sendCommand);
    }

    private async Task Send(GlobalOptions globalOptions, string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            json = await Console.In.ReadToEndAsync();

        var node = JsonNode.Parse(json);

        if (node is not JsonObject jsonObject)
        {
            Console.Error.WriteLine("Expected a JSON object.");
            return;
        }

        using var obs = await ConnectToObs(globalOptions);

        var responseObject = await obs.SendRaw(jsonObject);

        Console.WriteLine(responseObject);

    }
}
