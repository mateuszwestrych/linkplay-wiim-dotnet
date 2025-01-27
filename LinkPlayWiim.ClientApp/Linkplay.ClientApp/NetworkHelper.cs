using System.Net;
using System.Net.Sockets;

namespace Linkplay.ClientApp;

public static class NetworkHelper
{
    public static IPAddress GetIpAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.ToString().StartsWith("127.0.0.1"))
                continue;
            
            if (ip.AddressFamily == AddressFamily.InterNetwork)
                return ip;
        }
        
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
}