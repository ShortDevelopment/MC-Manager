using Windows.System;
using Windows.UI.Xaml.Controls;

namespace MC_Manager.Controls
{
    public sealed partial class LinkButtonControl : UserControl
    {
        public LinkButtonControl()
        {
            this.InitializeComponent();
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public System.Uri NavigationUri { get; set; }

        private void Button_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            _ = Launcher.LaunchUriAsync(NavigationUri);
        }
    }
}
