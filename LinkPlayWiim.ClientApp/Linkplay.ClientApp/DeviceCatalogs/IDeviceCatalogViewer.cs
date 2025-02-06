namespace Linkplay.ClientApp.DeviceCatalogs;

public interface IDeviceCatalogViewer
{
    IReadOnlyCollection<DeviceCatalogItem> GetDevices();
}