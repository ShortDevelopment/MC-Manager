namespace ShortDev.Minecraft.Bedrock.McWebSocket.Helpers
{
    public abstract class HelperBase
    {
        /// <summary>
        /// Gets the underlaying <see cref="ClientInstance"/>
        /// </summary>
        public ClientInstance Client { get; private set; }

        internal HelperBase(ClientInstance client)
        {
            this.Client = client;
        }
    }
}
