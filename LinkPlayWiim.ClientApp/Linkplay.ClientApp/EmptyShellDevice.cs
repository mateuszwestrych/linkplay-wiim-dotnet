using CSharpFunctionalExtensions;

namespace Linkplay.ClientApp;

public class EmptyShellDevice : IConnectableDevice
{
    public static EmptyShellDevice BuildFromNetworkAddress(string networkAddress) => new(networkAddress);
    private EmptyShellDevice(string networkAddress) => NetworkAddress = networkAddress;
    public string NetworkAddress { get; }
    public string? Identifier => null;

    public async Task<Result<Device>> Handshake(IDeviceConnector deviceConnector) 
        => await Device.Create(this, deviceConnector);
}