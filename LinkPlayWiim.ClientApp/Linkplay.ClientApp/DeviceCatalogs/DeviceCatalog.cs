using Linkplay.ClientApp.RestApiClient;

namespace Linkplay.ClientApp.DeviceCatalogs;

public class DeviceCatalog : IDeviceCatalogRegistrer, IDeviceCatalogViewer
{
    private readonly List<IConnectableDevice> _devices = new();
    public IReadOnlyCollection<IConnectableDevice> GetDevices() => _devices.AsReadOnly();
    public void RegisterDevice(IConnectableDevice device) => _devices.Add(device);
}