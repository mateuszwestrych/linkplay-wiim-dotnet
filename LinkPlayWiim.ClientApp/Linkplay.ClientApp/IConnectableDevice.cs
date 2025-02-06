namespace Linkplay.ClientApp;

public interface IConnectableDevice
{
    string NetworkAddress { get; }
    string? Identifier { get; }
}