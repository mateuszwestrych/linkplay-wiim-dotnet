using Newtonsoft.Json;

namespace Linkplay.Server;

public class PlayUrlRequest
{
    [JsonProperty("url")]
    public  string Url { get; set; }
}