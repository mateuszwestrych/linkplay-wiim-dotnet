using CSharpFunctionalExtensions;

namespace Linkplay.ClientApp.DeviceCatalogs;

public interface IDeviceCatalogFinder
{
    Result<Device> TryGetDevice(string deviceId);
}

public partial class DeviceCatalog : IDeviceCatalogRegistrer, IDeviceCatalogViewer, IDeviceCatalogFinder
{
    private readonly List<IConnectableDevice> _devices = new();
    public void RegisterDevice(IConnectableDevice device) => _devices.Add(device);
    
    public Result<Device> TryGetDevice(string deviceId)
    {
        var found = _devices.Find(device => device.Identifier == deviceId);
        
        if (found == null)
            return Result.Failure<Device>("Device not found");

        var found1 = found as Device;
        if (found1 == null)
            return Result.Failure<Device>("Device is wrong type");
        
        return Result.Success(found1);
    }

    public IConnectableDevice GetDevice(string deviceId)
    {
        throw new NotImplementedException();
    }
}