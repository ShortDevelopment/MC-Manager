using System;
using Windows.ApplicationModel;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace MC_Manager.Pages
{
    public sealed partial class WorldsPage : Page
    {
        public WorldsPage()
        {
            this.InitializeComponent();
            Initialize();
        }

        private async void Initialize()
        {
            if (DesignMode.DesignModeEnabled || DesignMode.DesignMode2Enabled)
                return;

            loadingIndicator.Visibility = Windows.UI.Xaml.Visibility.Visible;

            this.WorldInfos = await Worlds.WorldManager.GetWorldsAsync();
            worldsListView.ItemsSource = WorldInfos;

            loadingIndicator.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        public Worlds.WorldInfo[] WorldInfos { get; private set; }

        private async void OpenButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (worldsListView.SelectedIndex < 0)
                return;

            Worlds.WorldInfo[] items = worldsListView.ItemsSource as Worlds.WorldInfo[];
            await Launcher.LaunchFolderAsync(items[worldsListView.SelectedIndex].Folder);
        }

        private void DeleteButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (worldsListView.SelectedIndex < 0)
                return;

        }
    }
}
