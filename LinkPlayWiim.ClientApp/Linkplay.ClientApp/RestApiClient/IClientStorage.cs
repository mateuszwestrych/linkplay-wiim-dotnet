namespace Linkplay.ClientApp.RestApiClient;

public interface IClientStorage
{
    void SetDeviceEndpointAddress(string address);
}

public class ClientStorage : IClientStorage
{
    public void SetDeviceEndpointAddress(string address)
    {
        throw new NotImplementedException();
    }
}