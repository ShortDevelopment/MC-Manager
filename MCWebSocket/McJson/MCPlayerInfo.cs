using System;

namespace ShortDev.Minecraft.Bedrock.McWebSocket.McJson
{
    public struct MCPlayerInfo
    {
        public string activeSessionId;
        public string clientId;
        public string color;
        public string deviceSessionId;
        public string globalMultiplayerCorrelationId;
        public string name;
        public string randomId;
        public string uuid;

        public Guid GetID()
        {
            return Guid.Parse(uuid);
        }
    }
}
