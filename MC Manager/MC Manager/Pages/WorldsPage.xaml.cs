using System;
using Windows.ApplicationModel;
using Windows.System;
using Microsoft.UI.Xaml.Controls;
using ShortDev.Minecraft.Nbt;
using System.IO;
using System.Diagnostics;

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

            loadingIndicator.Visibility = Microsoft.UI.Xaml.Visibility.Visible;

            try
            {
                this.WorldInfos = await Worlds.WorldManager.GetWorldsAsync();
                worldsListView.ItemsSource = WorldInfos;
            }
            catch
            {
                MissingPermissionInfo.Visibility = Microsoft.UI.Xaml.Visibility.Visible;
            }

            loadingIndicator.Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
        }

        public Worlds.WorldInfo[] WorldInfos { get; private set; }

        private async void OpenButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (worldsListView.SelectedIndex < 0)
                return;

            Worlds.WorldInfo[] items = worldsListView.ItemsSource as Worlds.WorldInfo[];
            await Launcher.LaunchFolderAsync(items[worldsListView.SelectedIndex].Folder);
        }

        private void DeleteButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (worldsListView.SelectedIndex < 0)
                return;

        }

        private void EditNbtButton_Click(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            if (worldsListView.SelectedIndex < 0)
                return;

            Worlds.WorldInfo[] items = worldsListView.ItemsSource as Worlds.WorldInfo[];
            var folder = items[worldsListView.SelectedIndex].Folder;
            NbtEditWindow window = new(folder);
            window.Activate();
        }
    }
}
