using System.CommandLine;

namespace OBSWebSocket.CommandLine;

public class InputsCommands : CommandLineInterface
{
    protected override void RegisterCommands(RootCommand rootCommand, GlobalOptionsBinder globalOptionsBinder)
    {
        var inputsCommands = new Command("inputs", "Commands for controlling audio inputs.");

        var listInputsCommand = new Command("list", "List all audio inputs.");

        listInputsCommand.SetHandler(async globalOptions => { await ListAllInputs(globalOptions); },
            globalOptionsBinder);

        inputsCommands.AddCommand(listInputsCommand);

        rootCommand.AddCommand(inputsCommands);
    }

    private async Task ListAllInputs(GlobalOptions globalOptions)
    {
        using var obs = await ConnectToObs(globalOptions);

        var inputList = await obs.GetInputList();

        foreach (var input in inputList.ResponseData.Inputs)
        {
            Console.WriteLine(input.InputName);
        }
    }
}