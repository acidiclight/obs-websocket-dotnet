using System.CommandLine;

namespace OBSWebSocket.CommandLine;

public sealed class RecordingCommands : CommandLineInterface
{
    protected override void RegisterCommands(RootCommand rootCommand, GlobalOptionsBinder globalOptionsBinder)
    {
        var recordingCommands = new Command("recording", "Commands for controlling recording");

        var statusCommand = new Command("status", "Gets the status of recording.");
        statusCommand.SetHandler(async (globalOptions) =>
        {
            await GetRecordStatus(globalOptions);
        }, globalOptionsBinder);
        var isPausedCommand = new Command("is-paused", "Gets whether recording is paused.");
        isPausedCommand.SetHandler(async (globalOptions) =>
        {
            await GetIsPaused(globalOptions);
        }, globalOptionsBinder);
        
        var isActiveCommand = new Command("is-active", "Gets whether recording is currently active.");
        isActiveCommand.SetHandler(async (globalOptions) =>
        {
            await GetIsActive(globalOptions);
        }, globalOptionsBinder);
        

        recordingCommands.AddCommand(statusCommand);
        recordingCommands.AddCommand(isActiveCommand);
        recordingCommands.AddCommand(isPausedCommand);
        

        rootCommand.AddCommand(recordingCommands);
    }

    private async Task GetRecordStatus(GlobalOptions globalOptions)
    {
        using var obs = await ConnectToObs(globalOptions);

        var recordSTatus = await obs.GetRecordStatus();
        
        Console.WriteLine("active: {0}", recordSTatus.ResponseData.OutputActive);
        Console.WriteLine("paused: {0}", recordSTatus.ResponseData.OutputPaused);
        Console.WriteLine("timecode: {0}", recordSTatus.ResponseData.OutputTimecode);
        Console.WriteLine("duration: {0}", recordSTatus.ResponseData.OutputDuration);
        Console.WriteLine("bytes: {0}", recordSTatus.ResponseData.OutputBytes);
    }
    
    private async Task GetIsActive(GlobalOptions globalOptions)
    {
        using var obs = await ConnectToObs(globalOptions);

        var recordSTatus = await obs.GetRecordStatus();

        Console.WriteLine(recordSTatus.ResponseData.OutputActive);
    }
    
    private async Task GetIsPaused(GlobalOptions globalOptions)
    {
        using var obs = await ConnectToObs(globalOptions);

        var recordSTatus = await obs.GetRecordStatus();

        Console.WriteLine(recordSTatus.ResponseData.OutputPaused);
    }
    
    
}