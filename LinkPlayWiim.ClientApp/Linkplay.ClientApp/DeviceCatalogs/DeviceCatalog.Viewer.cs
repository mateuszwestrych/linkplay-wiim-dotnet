namespace Linkplay.ClientApp.DeviceCatalogs;

public partial class DeviceCatalog
{
    private List<DeviceCatalogItem>? _list;
    public IReadOnlyCollection<DeviceCatalogItem> GetDevices()
        => _list ??= BuildCatalog().ToList();
    
    private IEnumerable<DeviceCatalogItem> BuildCatalog()
    {
        foreach (var device in _devices)
        {
            if (device is Device d)
            {
                yield return new DeviceCatalogItem
                {
                    DeviceName = d.Name,
                    DeviceId = d.Identifier,
                    NetworkAddress = d.NetworkAddress
                };
            }
            else
            {
                yield return new DeviceCatalogItem
                {
                    NetworkAddress = device.NetworkAddress
                };
            }
        }
    }
}