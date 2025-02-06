using CSharpFunctionalExtensions;
using Linkplay.ClientApp.Abstracts;
using Linkplay.ClientApp.RestApiClient;

namespace Linkplay.ClientApp;

public interface IDeviceConnector
{
    Task<Result> ExecuteCommand(IConnectableDevice connectableDevice, DeviceCommand command, CancellationToken token);
    Task<Result<TCommandResult?>> ExecuteCommand<TCommandResult>(IConnectableDevice connectableDevice, DeviceCommand<TCommandResult> command) 
        where TCommandResult : class;
}

public class DeviceConnector : IDeviceConnector
{
    private readonly IRestApiClient _restApiClient;

    public DeviceConnector(IRestApiClient restApiClient)
    {
        _restApiClient = restApiClient;
    }

    public async Task<Result> ExecuteCommand(IConnectableDevice connectableDevice, DeviceCommand command, CancellationToken token)
    {
        var requestUrl = string.Concat(connectableDevice.NetworkAddress, "/httpapi.asp?command=", command.ToString());
        return await _restApiClient.GetAsync(requestUrl, token);
    }

    public async Task<Result<TCommandResult?>> ExecuteCommand<TCommandResult>(IConnectableDevice connectableDevice, DeviceCommand<TCommandResult> command) 
        where TCommandResult : class
    {
        var requestUrl = string.Concat(connectableDevice.NetworkAddress, "/httpapi.asp?command=", command.ToString());
        return await _restApiClient.GetAsync<TCommandResult>(requestUrl);
    }
}