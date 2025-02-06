using Linkplay.ClientApp.Abstracts;

namespace Linkplay.ClientApp.DeviceCommands;

public class DeviceStatusExCommand : DeviceCommand<DeviceStatusExResponse>
{
    public DeviceStatusExCommand() : base("getStatusEx")
    {
    }
}