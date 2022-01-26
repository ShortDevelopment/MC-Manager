using Newtonsoft.Json;
using System;

namespace ShortDev.Minecraft.Bedrock.McWebSocket.PackagePayloads
{
    public class CommandResponsePayload
    {
        [Obsolete("Use GetDetails method!")]
        public string details;

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("statusMessage")]
        public string StatusMessage { get; set; }

        public T GetDetails<T>()
        {
#pragma warning disable CS0618 // Typ oder Element ist veraltet
            return JsonConvert.DeserializeObject<T>(details);
#pragma warning restore CS0618 // Typ oder Element ist veraltet
        }
    }

}
