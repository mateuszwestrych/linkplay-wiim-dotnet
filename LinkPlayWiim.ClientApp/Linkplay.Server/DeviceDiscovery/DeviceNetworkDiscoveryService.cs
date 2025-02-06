using Linkplay.ClientApp;
using Linkplay.ClientApp.DeviceCatalogs;
using Linkplay.ClientApp.DeviceCommands;

namespace Linkplay.Server.DeviceDiscovery;

public class DeviceNetworkDiscoveryService : IHostedService, IDisposable
{
    public DeviceNetworkDiscoveryService(
        IDeviceCatalogRegistrer clientStorage,
        IDeviceCatalogViewer deviceCatalogViewer,
        ILogger<DeviceNetworkDiscoveryService> logger, 
        IDeviceConnector restApiClient)
    {
        _clientStorage = clientStorage;
        _deviceCatalogViewer = deviceCatalogViewer;
        _logger = logger;
        _restApiClient = restApiClient;
    }

  
    private readonly IDeviceCatalogRegistrer _clientStorage;
    private readonly IDeviceCatalogViewer _deviceCatalogViewer;
    private readonly ILogger _logger;
    private readonly IDeviceConnector _restApiClient;
    
    public void Dispose()
    {
        // TODO release managed resources here
    }

    private async Task CheckDeviceAvailabilityAsync(string address, CancellationTokenSource source, CancellationToken cancellationToken)
    {
        try
        {
            var emptyShellDevice = EmptyShellDevice.BuildFromNetworkAddress(address);

            var statusResult = await _restApiClient.ExecuteCommand(
                emptyShellDevice,
                new DeviceStatusExCommand(),
                cancellationToken);
            
            if (statusResult.IsSuccess)
            {
                var deviceHandshake = await emptyShellDevice.Handshake(_restApiClient);
                
                if (deviceHandshake.IsSuccess)
                    _clientStorage.RegisterDevice(deviceHandshake.Value);
                else
                    _clientStorage.RegisterDevice(emptyShellDevice);

                await source.CancelAsync();
            }
        }
        catch
        {
            //
        }
    }
    
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var ip = NetworkHelper.GetIpAddress();
        var lastDot = ip.ToString().LastIndexOf('.');
        var baseNetworkIpAddress = ip.ToString().Substring(0, lastDot);

        //CancellationTokenSource source = new CancellationTokenSource();
        var source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var tasks = new List<Task>();
        
        for (var i = 219; i <= 219; i++)
        {
            var checkIp = string.Concat("https://", baseNetworkIpAddress, ".", i.ToString());
            
            Console.WriteLine($"Checking IP: {checkIp}");
            
            if (!Uri.TryCreate(checkIp, UriKind.Absolute, out var uriResult))
                continue;
            
            tasks.Add(CheckDeviceAvailabilityAsync(uriResult.ToString(), source, source.Token));
        }
        
        Task.WaitAll(tasks.ToArray(), cancellationToken);

        var addresses = _deviceCatalogViewer.GetDevices();
        if (addresses.Any())
        {
            _logger.LogInformation("SUCCESS! Found devices.");
        }
        else
        {
            _logger.LogWarning("NO DEVICE AVAILABLE");
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
    }
}