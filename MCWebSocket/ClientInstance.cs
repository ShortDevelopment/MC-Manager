using ShortDev.Minecraft.Bedrock.McWebSocket.Helpers;
using ShortDev.Minecraft.Bedrock.McWebSocket.PackagePayloads;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShortDev.Minecraft.Bedrock.McWebSocket
{
    public class ClientInstance
    {
        /// <summary>
        /// Gets the <see cref="Server"/>
        /// </summary>
        public Server Server { get; private set; }

        /// <summary>
        /// Gets the underlaying <see cref="HttpListenerWebSocketContext"/>
        /// </summary>
        public HttpListenerWebSocketContext Context { get; private set; }

        internal ClientInstance(Server server, HttpListenerWebSocketContext context)
        {
            this.Server = server;
            this.Context = context;

            Task.Run(MessageLoop);

            InitializeHelpers();
        }

        #region MessageLoop

        private async void MessageLoop()
        {
            var ws = Context.WebSocket;

            while (ws.State == WebSocketState.Open)
            {
                Package package = await ReceivePackage(ws);
                //Debug.Print(JsonConvert.SerializeObject(package));
                if (package != null)
                {
                    if (package.Header.Purpose == "event")
                    {
                        // string eventName = package.Payload["eventName"].ToString();
                        ReceivedEventMessage?.Invoke(this, package);
                    }
                    else
                    {
                        if (promisQueue.ContainsKey(package.Header.ID))
                        {
                            promisQueue[package.Header.ID].SetResult(package);
                            promisQueue.Remove(package.Header.ID);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// https://stackoverflow.com/a/23784968
        /// </summary>
        /// <param name="ws"></param>
        /// <returns></returns>
        private async Task<Package> ReceivePackage(WebSocket ws)
        {
            ArraySegment<Byte> buffer = new ArraySegment<byte>(new Byte[8192]);
            WebSocketReceiveResult result = null;
            using (var ms = new MemoryStream())
            {
                do
                {
                    result = await ws.ReceiveAsync(buffer, CancellationToken.None);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    using (var reader = new StreamReader(ms, Encoding.UTF8))
                    {
                        return JsonConvert.DeserializeObject<Package>(reader.ReadToEnd());
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        /// <summary>
        /// Sends <see cref="Package"/>
        /// </summary>
        /// <param name="pack"></param>
        public async void SendPackage(Package pack)
        {
            var ws = Context.WebSocket;
            await ws.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(pack))), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        #region Events

        public event EventHandler<Package> ReceivedEventMessage;

        public void SubscribeEvent(string eventName)
        {
            SendPackage(new SubscribePackage(eventName));
        }

        #endregion

        #region Commands

        /// <summary>
        /// Send a <see cref="Package"/> and returns result <see cref="Package"/> from minecraft
        /// </summary>
        /// <param name="pack"></param>
        /// <returns></returns>
        public Task<Package> SendPackageWait(Package pack)
        {
            SendPackage(pack);
            var promise = new TaskCompletionSource<Package>();
            //Debug.Print(pack.Header.ID);
            promisQueue.Add(pack.Header.ID, promise);
            return promise.Task;
        }

        private Dictionary<string, TaskCompletionSource<Package>> promisQueue = new Dictionary<string, TaskCompletionSource<Package>>();

        public Task<Package> ExecuteCommand(string commandLine)
        {
            return SendPackageWait(new CommandRequestPackage(commandLine));
        }

        #endregion

        #region Helpers

        private void InitializeHelpers()
        {
            InternalCommands = new InternalCommandsHelper(this);
            Players = new PlayerHelper(this);
        }

        public InternalCommandsHelper InternalCommands { get; private set; }
        public PlayerHelper Players { get; private set; }

        public async void CreateHelpFile()
        {
            int pageCount = (int)(await ExecuteCommand("help")).Payload["pageCount"];

            string cmds = string.Join("\n", await Task.WhenAll(new string[pageCount].Select(async (x, pi) =>
            {
                var pack = await ExecuteCommand($"help {pi + 1}");
                return pack.Payload["body"] + "\n";
            })));

            Func<string, string, string[]> Split = (string str, string seperator) => str.Split(new[] { seperator }, StringSplitOptions.None);

            string content = string.Join("\n", await Task.WhenAll(Split(cmds, "\n").Select(async (full_cmd) =>
            {
                if (!string.IsNullOrEmpty(full_cmd))
                {
                    StringBuilder builder = new StringBuilder();
                    string cmd = Split(full_cmd, " ")[0].Replace("/", "");
                    var pack = await ExecuteCommand($"help {cmd}");
                    builder.AppendLine($"## {cmd}");
                    builder.AppendLine($"`{full_cmd}`");
                    builder.AppendLine(pack.Payload["statusMessage"].ToString());
                    return builder.ToString();
                }
                else
                {
                    return null;
                }
            })));

            System.IO.File.WriteAllText("help.md", content);
        }

        #endregion

    }
}
