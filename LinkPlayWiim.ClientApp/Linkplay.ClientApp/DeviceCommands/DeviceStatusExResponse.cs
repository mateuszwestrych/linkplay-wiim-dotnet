using Newtonsoft.Json;

namespace Linkplay.ClientApp.DeviceCommands;

public class DeviceStatusExResponse
{
    [JsonProperty("deviceName")]
    public string DeviceName { get; set; }
    
    [JsonProperty("identifier")]
    public string Identifier { get; set; }
}