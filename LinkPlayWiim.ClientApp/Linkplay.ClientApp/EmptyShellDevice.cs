namespace Linkplay.ClientApp;

public class EmptyShellDevice : IConnectableDevice
{
    public static EmptyShellDevice BuildFromNetworkAddress(string networkAddress) => new(networkAddress);
    private EmptyShellDevice(string networkAddress) => NetworkAddress = networkAddress;
    public string NetworkAddress { get; private set; }
}