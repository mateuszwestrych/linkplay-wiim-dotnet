namespace Linkplay.ClientApp.RestApiClient;

public interface IDeviceCatalogViewer
{
    IReadOnlyCollection<IConnectableDevice> GetDevices();
}