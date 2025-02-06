using Linkplay.ClientApp;
using Linkplay.ClientApp.DeviceCommands;
using Linkplay.ClientApp.RestApiClient;

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

            var result = await _restApiClient.ExecuteCommand(
                emptyShellDevice,
                new DeviceStatusExCommand(),
                cancellationToken);
            
            if (result.IsSuccess)
            {
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
        var baseIpAddress = ip.ToString().Substring(0, lastDot);

        //CancellationTokenSource source = new CancellationTokenSource();
        var source = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var tasks = new List<Task>();
        
        for (var i = 1; i <= 255; i++)
        {
            var checkIp = string.Concat("https://",baseIpAddress,".",i.ToString(),"/httpapi.asp?command=getStatusEx");
            
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