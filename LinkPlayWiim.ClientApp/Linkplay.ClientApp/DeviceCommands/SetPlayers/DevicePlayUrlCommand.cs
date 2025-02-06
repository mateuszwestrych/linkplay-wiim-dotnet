using System.Web;
using CSharpFunctionalExtensions;
using Linkplay.ClientApp.Abstracts;

namespace Linkplay.ClientApp.DeviceCommands.SetPlayers;

public class DevicePlayUrlCommand : DeviceCommand
{
    public static Result<DevicePlayUrlCommand> Create(string url)
    {
        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            return Result.Failure<DevicePlayUrlCommand>("Invalid URL");
        
        return Result.Success(new DevicePlayUrlCommand(HttpUtility.UrlEncode(url)));
    }
    private DevicePlayUrlCommand(string url) : base("setPlayerCmd","url") 
        => Value = url;
}