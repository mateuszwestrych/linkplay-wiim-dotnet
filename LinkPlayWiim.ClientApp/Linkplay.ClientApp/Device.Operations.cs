using CSharpFunctionalExtensions;
using Linkplay.ClientApp.DeviceCommands;

namespace Linkplay.ClientApp;

public partial class Device
{
    public static async Task<Result<Device>> Create(EmptyShellDevice emptyShellDevice, IDeviceConnector deviceConnector)
    {
        var cmd = await deviceConnector.ExecuteCommand(emptyShellDevice, new DeviceStatusExCommand());

        return cmd.Bind<DeviceStatusExResponse, Device>(res =>
            new Device(
                identifier: res.Identifier,
                name: res.DeviceName,
                networkAddress: emptyShellDevice.NetworkAddress,
                knownVolume: 0));
    }
}