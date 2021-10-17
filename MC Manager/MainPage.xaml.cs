using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NavigationViewItem = Microsoft.UI.Xaml.Controls.NavigationViewItem;

namespace MC_Manager
{
    public sealed partial class MainPage : Page
    {

        CoreApplicationViewTitleBar coreTitleBar;
        public MainPage()
        {
            this.InitializeComponent();

            Window.Current.SetTitleBar(AppTitleBar);

            // https://docs.microsoft.com/de-de/windows/apps/design/style/mica

            coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;
            coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

            rootFrame.Navigate(typeof(Pages.HomePage));
        }

        private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            //Thickness currMargin = AppTitleBar.Margin;
            //AppTitleBar.Margin = new Thickness(currMargin.Left, currMargin.Top, coreTitleBar.SystemOverlayRightInset, currMargin.Bottom);
        }

        private void NavigationViewControl_ItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            NavigationViewItem item = args.InvokedItemContainer as NavigationViewItem;
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
                        _ = Launcher.LaunchFolderPathAsync(System.IO.Path.Combine(Utils.UserLocalAppData, @"Packages\Microsoft.MinecraftUWP_8wekyb3d8bbwe\LocalState\games\com.mojang"));
                    }
                    catch { }
                    break;
                case "open_docs":
                    try
                    {
                        _ = Launcher.LaunchUriAsync(new System.Uri("https://bedrock.dev/"));
                    }
                    catch { }
                    break;
                case "show_links":
                    rootFrame.Navigate(typeof(Pages.LinksPage));
                    break;
            }
        }
    }
}
