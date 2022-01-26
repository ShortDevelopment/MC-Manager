using ShortDev.Minecraft.Bedrock.McWebSocket.McJson;
using ShortDev.Minecraft.Bedrock.McWebSocket.PackagePayloads;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace ShortDev.Minecraft.Bedrock.McWebSocket.Helpers
{
    public class PlayerHelper : HelperBase
    {

        internal PlayerHelper(ClientInstance client) : base(client) { }

        public async Task<MCEntityInfo[]> GetPlayersInfo(string selector = "@s")
        {
            var data = await Client.InternalCommands.QueryTarget(selector);
            return data.GetPayload<CommandResponsePayload>().GetDetails<MCEntityInfo[]>();
        }

        public async Task<MCPlayerInfo[]> GetPlayers()
        {
            var data = await Client.InternalCommands.ListD();
            var details = (JObject)JsonConvert.DeserializeObject(data.GetPayload<CommandResponsePayload>().details.Replace("###*", "").Replace("*###", ""));
            return details["result"].ToObject<MCPlayerInfo[]>();
        }

        public async Task<int> GetCurrentPlayerCount()
        {
            var data = await Client.InternalCommands.ListD();
            return (int)data.Payload["currentPlayerCount"];
        }

        public async Task<int> GetMaxPlayerCount()
        {
            var data = await Client.InternalCommands.ListD();
            return (int)data.Payload["maxPlayerCount"];
        }

    }
}
