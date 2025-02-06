using Newtonsoft.Json;

namespace Linkplay.ClientApp.DeviceCommands;

public class DeviceStatusExResponse
{
    [JsonProperty("deviceName")]
    public string DeviceName { get; set; }
    
    [JsonProperty("uuid")]
    public string Identifier { get; set; }
}