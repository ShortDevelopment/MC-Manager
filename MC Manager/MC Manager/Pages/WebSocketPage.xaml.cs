﻿using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using ShortDev.Minecraft.Bedrock.McWebSocket;
using ShortDev.Minecraft.Bedrock.McWebSocket.PackagePayloads;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel;
using Windows.ApplicationModel.DataTransfer;
using Windows.Networking.Sockets;

namespace MC_Manager.Pages
{
    public sealed partial class WebSocketPage : Page
    {
        public WebSocketPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
            Initialize();
        }

        public Server WebSocketServer { get; private set; }
        private void Initialize()
        {
            if (DesignMode.DesignModeEnabled || DesignMode.DesignMode2Enabled)
                return;

            WebSocketServer = new Server();
            WebSocketServer.Start();
            WebSocketServer.NewClient += WebSocketServer_NewClient;
        }

        List<ClientInstance> Clients = new();
        private void WebSocketServer_NewClient(object sender, ClientInstance client)
        {
            client.InternalCommands.SendChatMessage("[§bBot§r]: Connected WebSocket!", "@s");
            client.ReceivedEventMessage += Client_ReceivedEventMessage;
            Clients.Add(client);

            foreach (var eventName in SubscribePackage.WellKnownEvents)
                client.SubscribeEvent(eventName);
        }

        private void Client_ReceivedEventMessage(object sender, ShortDev.Minecraft.Bedrock.McWebSocket.Package payload)
        {
            _ = DispatcherQueue.TryEnqueue(() =>
            {
                string text = IncomingTextBox.Text;
                IncomingTextBox.Text = Truncate(text, 1000) + Environment.NewLine + Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine + payload.GetJSON();
            });
        }

        private string Truncate(string input, int length)
        {
            int inputLength = input.Length;
            if (inputLength <= length)
                return input;
            return input.Substring(inputLength - length, length);
        }

        private async void SendAppBarButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            string cmd = CommandTextBox.Text;
            CommandTextBox.Text = "";

            var clients = this.Clients.ToArray();
            foreach (var client in clients)
            {
                try
                {
                    var package = await client.ExecuteCommand(cmd);
                    Client_ReceivedEventMessage(null, package);
                }
                catch
                {
                    Clients.Remove(client);
                }
            }
        }

        private void CommandTextBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                SendAppBarButton_Click(null, null);
        }

        private void CopyButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            DataPackage package = new();
            package.SetText(ConnectionStringTextBox.Text);
            Clipboard.SetContent(package);
        }
    }
}
