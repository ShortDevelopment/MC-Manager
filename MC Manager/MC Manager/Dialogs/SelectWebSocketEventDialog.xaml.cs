using Microsoft.UI.Xaml.Controls;
using ShortDev.Minecraft.Bedrock.McWebSocket.PackagePayloads;

namespace MC_Manager.Dialogs
{
    public sealed partial class SelectWebSocketEventDialog : ContentDialog
    {
        public SelectWebSocketEventDialog()
        {
            this.InitializeComponent();

            EventSelectComboBox.ItemsSource = SubscribePackage.WellKnownEvents;
        }
    }
}
