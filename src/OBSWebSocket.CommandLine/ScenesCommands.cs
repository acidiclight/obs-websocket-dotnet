using System.CommandLine;
using OBSWebSocket.Client;

namespace OBSWebSocket.CommandLine;

public sealed class ScenesCommands : CommandLineInterface
{
    protected override void RegisterCommands(RootCommand rootCommand, GlobalOptionsBinder globalOptionsBinder)
    {
        var nameArgument = new Argument<string>("name", "Specifies a scene name");
        
        var scenesCommand = new Command("scenes",
            "Commands for manahing scenes.");

        var listScenesCommand = new Command("list",
            "Prints a list of all available scenes.");
        listScenesCommand.SetHandler(async (globalOptions) =>
        {
            await ListScenes(globalOptions);
        }, globalOptionsBinder);

        var getCurrentSceneCommand = new Command("get-current",
            "Prints the name of the current scene.");
        getCurrentSceneCommand.SetHandler(async (globalOptions) =>
        {
            await GetCurrentScene(globalOptions);
        }, globalOptionsBinder);
        
        var getPreviewSceneCommand = new Command("get-preview",
            "If in Studio Mode, prints the current preview scene. Otherwise, this command prints nothing.");
        getCurrentSceneCommand.SetHandler(async (globalOptions) =>
        {
            await GetPreviewScene(globalOptions);
        }, globalOptionsBinder);
     
        var setCurrentSceneNameCommand = new Command("set-current",
            "Sets the current scene to the scene name specified.");
        
        setCurrentSceneNameCommand.AddArgument(nameArgument);
        getCurrentSceneCommand.SetHandler(async (globalOptions, name) =>
        {
            await SetCurrentScene(globalOptions, name);
        }, globalOptionsBinder, nameArgument);
     
        
        
        scenesCommand.AddCommand(listScenesCommand);
        scenesCommand.Add(getCurrentSceneCommand);
        scenesCommand.Add(getPreviewSceneCommand);
        scenesCommand.Add(setCurrentSceneNameCommand);
        
        rootCommand.AddCommand(scenesCommand);
    }
    
    private async Task ListScenes(GlobalOptions globalOptions)
    {
        using ObsClient obs = await ConnectToObs(globalOptions);

        var sceneList = await obs.GetScenesList();

        foreach (var scene in sceneList.ResponseData.Scenes.OrderBy(x=>x.SceneIndex))
        {
            Console.WriteLine(scene.SceneName);
        }
    }
    
    private async Task GetCurrentScene(GlobalOptions globalOptions)
    {
        using ObsClient obs = await ConnectToObs(globalOptions);

        var sceneList = await obs.GetScenesList();

        Console.WriteLine(sceneList.ResponseData.CurrentProgramSceneName);
    }
    
    private async Task GetPreviewScene(GlobalOptions globalOptions)
    {
        using ObsClient obs = await ConnectToObs(globalOptions);

        var sceneList = await obs.GetScenesList();

        Console.WriteLine(sceneList.ResponseData.CurrentPreviewSceneName);
    }
    
    private async Task SetCurrentScene(GlobalOptions globalOptions, string name)
    {
        using ObsClient obs = await ConnectToObs(globalOptions);

        await obs.SetCurrentProgramScene(name);
    }
}