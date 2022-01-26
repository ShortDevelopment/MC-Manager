using System;
using System.Collections.Generic;
using System.Net;

namespace ShortDev.Minecraft.Bedrock.McWebSocket
{
    public class Server
    {

        /// <summary>
        /// Gets the current server port
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Gets the underlaying <see cref="HttpListener"/> used for <see cref="McWebSocket"/>
        /// </summary>
        public HttpListener Listener { get; private set; }

        /// <summary>
        /// Gets a list of current connections
        /// </summary>
        public List<ClientInstance> Connections { get; private set; } = new List<ClientInstance>();

        public Server() : this(3000) { }
        public Server(int port)
        {
            this.Port = port;
        }

        public void Start()
        {
            Listener = new HttpListener();
            Listener.Prefixes.Add($"http://localhost:{Port}/");
            Listener.Start();            

            Listener.BeginGetContext(async (IAsyncResult result) =>
            {
                HttpListenerContext context = Listener.EndGetContext(result);
                if (context.Request.IsWebSocketRequest)
                {
                    var wsContext = await context.AcceptWebSocketAsync(null);

                    var client = new ClientInstance(this, wsContext);
                    Connections.Add(client);
                    NewClient?.Invoke(this, client);
                }
            }, null);
        }

        public event EventHandler<ClientInstance> NewClient;
    }
}
