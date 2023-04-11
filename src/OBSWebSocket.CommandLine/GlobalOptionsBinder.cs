using System.CommandLine;
using System.CommandLine.Binding;

namespace OBSWebSocket.CommandLine;

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