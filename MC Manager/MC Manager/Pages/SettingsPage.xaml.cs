using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using Windows.Storage.Pickers;
using WinRT;
using WinUI.Interop.CoreWindow;
using WinUI.Interop.NativeWindow;

namespace MC_Manager.Pages
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
            MinecraftFolderPathTextBox.Text = Utils.MinecraftPath;
        }

        private async void PickMinecraftFolder_Click(object sender, RoutedEventArgs e)
        {
            FolderPicker picker = new();
            picker.FileTypeFilter.Add("*");
            picker.As<IInitializeWithWindow>().Initialize(MainWindow.Current.GetHandle());
            var folder = await picker.PickSingleFolderAsync();
            if (folder != null)
            {
                MinecraftFolderPathTextBox.Text = folder.Path;
                Utils.MinecraftPath = folder.Path;
            }
        }

        public string UI_CurrentYear { get; } = DateTime.Now.ToString("yyyy");
        public Version UI_VersionInfo { get; } = typeof(Program).Assembly.GetName().Version;

        private void ClearMinecraftFolderButton_Click(object sender, RoutedEventArgs e)
        {
            Utils.MinecraftPath = null;
            MinecraftFolderPathTextBox.Text = Utils.MinecraftPath;
        }
    }
}
