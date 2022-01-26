using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace ShortDev.Minecraft.Bedrock.McWebSocket.Helpers
{
    public class InternalCommandsHelper : HelperBase
    {

        internal InternalCommandsHelper(ClientInstance client) : base (client) { }

        public Task<Package> GetEduClientInfo()
        {
            return Client.ExecuteCommand("geteduclientinfo");
        }

        public Task<Package> ListD()
        {
            return Client.ExecuteCommand("listd");
        }

        public Task<Package> CloseChat()
        {
            return Client.ExecuteCommand("closechat");
        }

        public Task<Package> GetLocalPlayerName()
        {
            return Client.ExecuteCommand("getlocalplayername");
        }

        public Task<Package> QueryTarget(string selector = "@s")
        {
            return Client.ExecuteCommand($"querytarget {selector}");
        }

        public Task<Package> GetTopSolidBlock(string x = "~", string y = "~", string z = "~")
        {
            return Client.ExecuteCommand($"gettopsolidblock {x} {y} {z}");
        }

        public Task<Package> SendChatMessage(string msg, string selector = "@a")
        {
            var data = new JObject();
            data["rawtext"] = new JArray();
            ((JArray)data["rawtext"]).Add(new JObject());
            data["rawtext"][0]["text"] = msg;
            return Client.ExecuteCommand($"/tellraw {selector} {JsonConvert.SerializeObject(data)}");
        }

    }
}
