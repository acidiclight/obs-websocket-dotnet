namespace OBSWebSocket.Client;

public enum EventSubscription
{
    None,
    General,
    Config,
    Scenes,
    Inputs,
    Transitions,
    Filters,
    Outputs,
    SceneItems,
    MediaInputs,
    Vendors,
    Ui,
    All,
    InputVolumeMeters,
    InputActiveStateChanged,
    InputShowStateChanged,
    SceneItemTransformChanged
}