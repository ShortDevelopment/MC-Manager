﻿using Microsoft.UI.Xaml.Controls;
using System.Linq;
using Windows.Management.Deployment;
using Windows.System;
using NavigationViewItem = Microsoft.UI.Xaml.Controls.NavigationViewItem;

namespace MC_Manager
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            rootFrame.Navigate(typeof(Pages.HomePage));
        }

        private void NavigationViewControl_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            NavigationViewItem item = args.InvokedItemContainer as NavigationViewItem;

            if (args.IsSettingsInvoked)
                rootFrame.Navigate(typeof(Pages.SettingsPage));
            if (item?.Tag == null)
                return;

            string tag = item.Tag as string;
            switch (tag)
            {
                case "home":
                    rootFrame.Navigate(typeof(Pages.HomePage));
                    break;

                case "manage_worlds":
                    rootFrame.Navigate(typeof(Pages.WorldsPage));
                    break;
                case "portfolio":
                    rootFrame.Navigate(typeof(Pages.PortfolioPage));
                    break;

                case "websocket":
                    rootFrame.Navigate(typeof(Pages.WebSocketPage));
                    break;

                case "log_win10":
                    rootFrame.Navigate(typeof(Pages.LogPage), Logs.LogManager.McClientLogPath);
                    break;
                case "log_server":
                    rootFrame.Navigate(typeof(Pages.LogPage), Logs.LogManager.McServerLogPath);
                    break;

                case "server_setup":
                    rootFrame.Navigate(typeof(Pages.ServerSetupPage));
                    break;

                case "open_com_mojang_folder":
                    try
                    {
                        _ = Launcher.LaunchFolderPathAsync(System.IO.Path.Combine(Utils.MinecraftPath, @"LocalState\games\com.mojang"));
                    }
                    catch { }
                    break;
                case "open_install_location":
                    try
                    {
                        OpenInstallLocation();
                    }
                    catch { }
                    break;
                case "open_docs":
                    try
                    {
                        _ = Launcher.LaunchUriAsync(new System.Uri("https://learn.microsoft.com/en-us/minecraft/creator/"));
                    }
                    catch { }
                    break;
                case "show_links":
                    rootFrame.Navigate(typeof(Pages.LinksPage));
                    break;
            }
        }

        void OpenInstallLocation()
        {
            PackageManager pm = new();
            var package = pm.FindPackagesForUser(string.Empty, "Microsoft.MinecraftUWP_8wekyb3d8bbwe").FirstOrDefault();
            _ = Launcher.LaunchFolderAsync(package.InstalledLocation);
        }
    }
}
