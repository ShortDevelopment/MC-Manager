using ShortDev.Minecraft.Bedrock.McWebSocket;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.UI.Xaml.Controls;

namespace MC_Manager.Pages
{
    public sealed partial class WebSocketPage : Page
    {
        public WebSocketPage()
        {
            this.InitializeComponent();
            Initialize();
        }

        public Server WebSocketServer { get; private set; }
        private async void Initialize()
        {
            if (DesignMode.DesignModeEnabled || DesignMode.DesignMode2Enabled)
                return;

            WebSocketServer = new Server();
            WebSocketServer.Start();
            WebSocketServer.NewClient += WebSocketServer_NewClient;
        }

        private void Listener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            
        }

        private void WebSocketServer_NewClient(object sender, ClientInstance e)
        {
            e.InternalCommands.SendChatMessage("[§bBot§r]: Connected!", "@s");
        }
    }
}
