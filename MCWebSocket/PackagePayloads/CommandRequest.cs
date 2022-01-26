using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using static ShortDev.Minecraft.Bedrock.McWebSocket.PackagePayloads.CommandRequestPackage;

namespace ShortDev.Minecraft.Bedrock.McWebSocket.PackagePayloads
{

    public class CommandRequestPackage : Package<CommandRequestPayload>
    {

        public CommandRequestPackage(string commandLine)
        {
            Header.ID = Guid.NewGuid().ToString();
            Header.Type = "commandRequest";
            Header.Purpose = "commandRequest";

            Payload = new CommandRequestPayload();
            Payload.CommandLine = commandLine;
            Payload.Origin = new JObject();
            Payload.Origin["type"] = "player";
        }

        public class CommandRequestPayload
        {
            [JsonProperty("version")]
            public int Version { get; set; } = 1;

            [JsonProperty("commandLine")]
            public string CommandLine { get; set; }

            [JsonProperty("origin")]
            public JObject Origin { get; set; }
        }
    }

}
