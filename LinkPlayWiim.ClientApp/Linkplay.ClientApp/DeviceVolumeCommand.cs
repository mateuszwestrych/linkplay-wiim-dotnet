using CSharpFunctionalExtensions;
using Linkplay.ClientApp.Abstracts;

namespace Linkplay.ClientApp.RestApiClient;

public class DeviceVolumeCommand : DeviceCommand
{
    public int Volume;
    private DeviceVolumeCommand(int volume) : base("setPlayerCmd","vol")
    {
        Volume = volume;
        Value = volume.ToString();
    }

    public static Result<DeviceVolumeCommand> MaxVolume() =>  Result.Success(new DeviceVolumeCommand(100));

    public static DeviceVolumeCommand MinVolume() => new (0);

    public static DeviceVolumeCommand SetVolume(int volume) => new(volume);
}