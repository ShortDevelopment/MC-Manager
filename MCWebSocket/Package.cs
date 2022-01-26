using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace ShortDev.Minecraft.Bedrock.McWebSocket
{
    public class Package<PayloadType> : Package
    {
        [JsonProperty("body")]
        public new PayloadType Payload { get; set; }
    }

    public class Package
    {
        [JsonProperty("header")]
        public PackageHeader Header { get; set; } = new PackageHeader();

        public class PackageHeader
        {
            [JsonProperty("version")]
            public int Version { get; set; } = 1;

            [JsonProperty("requestId")]
            public string ID { get; set; }

            [JsonProperty("messageType")]
            public string Type { get; set; }

            [JsonProperty("messagePurpose")]
            public string Purpose { get; set; }
        }

        [JsonProperty("body"), Obsolete("Use GetPayload method!")]
        public JObject Payload { get; set; }

        public T GetPayload<T>()
        {
#pragma warning disable CS0618 // Typ oder Element ist veraltet
            return Payload.ToObject<T>();
#pragma warning restore CS0618 // Typ oder Element ist veraltet
        }

        public string GetJSON(Formatting formatting = Formatting.Indented)
        {
            return JsonConvert.SerializeObject(this, formatting);
        }

    }
}
