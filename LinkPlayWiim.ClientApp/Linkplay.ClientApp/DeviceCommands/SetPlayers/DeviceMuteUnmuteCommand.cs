using Linkplay.ClientApp.Abstracts;

namespace Linkplay.ClientApp.DeviceCommands.SetPlayers;

public class DeviceMuteUnmuteCommand : DeviceCommand
{
    private DeviceMuteUnmuteCommand(bool mute) : base("setPlayerCmd","mute", mute ? "1" :"0")
    {
    }

    public static DeviceMuteUnmuteCommand Mute() => new(true);
    public static DeviceMuteUnmuteCommand Unmute() => new(false);
}