using System.Windows;

namespace ShortDev.Minecraft.Bedrock.Debug
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitializeWebView();
            SubscribeWebServer();
        }

        async void InitializeWebView()
        {
            await WebView.EnsureCoreWebView2Async();
            WebView.CoreWebView2.SetVirtualHostNameToFolderMapping("overlay.local", "overlay/public", Microsoft.Web.WebView2.Core.CoreWebView2HostResourceAccessKind.Allow);
            WebView.Source = new System.Uri("https://overlay.local/index.html");
        }

        void SubscribeWebServer()
        {
            WebServer.OnDataReceived += SendData;
        }

        private void SendData(string data)
        {
            Dispatcher.BeginInvoke(() =>
            {
                WebView.CoreWebView2.PostWebMessageAsJson(data);
            });
        }
    }
}
