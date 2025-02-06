namespace Linkplay.ClientApp.Abstracts;

public abstract class DeviceCommand
{
    protected DeviceCommand(params string[] initialParams)
        => Command = string.Join(':',initialParams);

    private string Command { get; }
    protected string? Value { get; set; }
    
    public override string ToString() 
       => Value != null ? string.Concat(Command, ':', Value) : Command;
}


public abstract class DeviceCommand<TCommandResult> : DeviceCommand
    where TCommandResult : class
{
    protected DeviceCommand(params string[] initialParams) : base(initialParams)
    {
    }
}