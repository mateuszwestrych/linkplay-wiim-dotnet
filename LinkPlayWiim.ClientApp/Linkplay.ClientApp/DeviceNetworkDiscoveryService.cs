using Linkplay.ClientApp.RestApiClient;

namespace Linkplay.ClientApp;

public class DeviceNetworkDiscoveryService : IHostedService, IDisposable
{
    public DeviceNetworkDiscoveryService(IHttpClientFactory httpClientFactory, IClientStorage clientStorage)
    {
        _clientStorage = clientStorage;
        _httpClient = httpClientFactory.CreateClient("WiimApiClient");
    }

    private readonly HttpClient _httpClient;
    private readonly IClientStorage _clientStorage;

    public void Dispose()
    {
        // TODO release managed resources here
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var ip = NetworkHelper.GetIpAddress();
        var lastDot = ip.ToString().LastIndexOf('.');
        var baseIpAddress = ip.ToString().Substring(0, lastDot);

        for (var i = 0; i <= 255; i++)
        {
            var checkIp = string.Concat("https://",baseIpAddress,".",i.ToString(),"/httpapi.asp?command=getStatusEx");
            if (!Uri.TryCreate(checkIp, UriKind.Absolute, out var uriResult))
                continue;
            var response = await _httpClient.GetAsync(uriResult, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                _clientStorage.SetDeviceEndpointAddress(uriResult.ToString());
            }
            Console.WriteLine($"Checking IP: {checkIp}");
        }

    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}