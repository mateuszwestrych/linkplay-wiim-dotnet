using CSharpFunctionalExtensions;
using Linkplay.ClientApp.DeviceCommands.SetPlayers;
using Linkplay.ClientApp.RestApiClient;

namespace Linkplay.ClientApp;

public partial class Device : IConnectableDevice
{
    private Device(string identifier, string name, string networkAddress, int? knownVolume)
    {
        Identifier = identifier;
        Name = name;
        NetworkAddress = networkAddress;
        _knownVolume = knownVolume ?? 10;
    }

    public string Identifier { get; private set; }
    public string Name { get; private set; }
    public string NetworkAddress { get; private set; }

    private int _knownVolume;
    private readonly int _volumeSteps = 5;

    public async Task<Result> VolumeUp(IDeviceConnector deviceConnector, CancellationToken cancellationToken)
    {
        var command = DeviceVolumeCommand.SetVolume(_knownVolume += _volumeSteps);
        return await deviceConnector.ExecuteCommand(this, command, cancellationToken);
    }

    public async Task<Result> VolumeDown(IDeviceConnector deviceConnector, CancellationToken cancellationToken)
    {
        var command = DeviceVolumeCommand.SetVolume(_knownVolume -= _volumeSteps);
        return await deviceConnector.ExecuteCommand(this, command, cancellationToken);
    }

    public async Task<Result> Mute(IDeviceConnector deviceConnector, CancellationToken cancellationToken)
    {
        var command = DeviceMuteUnmuteCommand.Mute();
        return await deviceConnector.ExecuteCommand(this, command, cancellationToken);
    }

    public async Task<Result> Unmute(IDeviceConnector deviceConnector, CancellationToken cancellationToken)
    {
        var command = DeviceMuteUnmuteCommand.Unmute();
        return await deviceConnector.ExecuteCommand(this, command, cancellationToken);
    }
}